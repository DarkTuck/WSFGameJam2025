using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class GeneratorScript : MonoBehaviour
    {
        int id;
        private Coroutine timeToFail,timeToDestroy;
        [SerializeField]Vector2 timeLimit;
        [SerializeField] private int timeToDestroyMultiplayer;
        [SerializeField] private QTEScript qteScript;
        [SerializeField] private GameObject[] batterySegments;
        private SpriteRenderer[] batterySprites;
        Inputs inputs;
        private bool readyToRepair,playerInRange;
        void Awake()
        {
            inputs = new Inputs();
        }

        void OnEnable()
        {
            inputs.Player.Enable();
            inputs.Player.Interact.started+=StartRepair;
        }

        void OnDisable()
        {
            inputs.Player.Disable();
            inputs.Player.Interact.started -= StartRepair;
        }
        void Start()
        {
            id = GeneratorsManager.RegisterGenerator();
            batterySprites = new SpriteRenderer[batterySegments.Length];
            for (int i = 0; i < batterySegments.Length; i++)
            {
                batterySprites[i] = batterySegments[i].GetComponent<SpriteRenderer>();
            } 
            qteScript.enabled = false;
            timeToFail = StartCoroutine(TimeToFail());
        }

        void StartRepair(InputAction.CallbackContext context)
        {
            if(!playerInRange)return;
            StopCoroutine(timeToDestroy);
            timeToFail = null;
            qteScript.enabled = true;
        }

       public void SuccessRepair()
        {
            timeToFail = StartCoroutine(TimeToFail());
            readyToRepair=false;
            qteScript.enabled = false;
            GeneratorsManager.RegisterRepair();
        }

        public void FailRepair()
        {
            ChangeState(false);
        }
        IEnumerator TimeToFail()
        {
            float time = Random.Range(timeLimit.x, timeLimit.y);
            float timeLeft = time;
            while (timeLeft>=0)
            {
                timeLeft-=Time.deltaTime;
                ChangeColorByState(time, timeLeft);
                yield return null;
            }
            timeToDestroy = StartCoroutine(TimeToDestroy());
            //inputs.Player.Enable();
            readyToRepair = true;
            Debug.Log("Ready to repair");
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!readyToRepair || !other.CompareTag("Player")) return;
            Debug.Log("player entered");
            playerInRange = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!readyToRepair || !other.CompareTag("Player")) return;
            playerInRange = false;
        }

        IEnumerator TimeToDestroy()
        {
            float time = Random.Range(timeLimit.x, timeLimit.y)*timeToDestroyMultiplayer;
            float timeLeft = time;
            while (timeLeft>=0)
            {
                timeLeft-=Time.deltaTime;
                yield return null;
            }
            FailRepair();
            //ChangeState(false);
        }
        void ChangeState(bool state)
        {
            GeneratorsManager.ChangeState(id,state);
        }
        void ChangeColorByState(float baseValue,float currentValue)
        {
            float percent = currentValue/baseValue;
            Debug.Log(percent);
            switch (percent)
            {
                case var _ when percent is > 0.0f and < 0.25f:
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        if (i < 1)
                        {
                            batterySegments[i].SetActive(true);
                            batterySprites[i].color=Color.red;
                            continue;
                        }
                        batterySegments[i].SetActive(false);
                    }
                    break;
                case var _ when percent is > 0.25f and < 0.50f:
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        if (i < 2)
                        {
                            batterySegments[i].SetActive(true);
                            batterySprites[i].color=Color.yellow;
                            continue;
                        }
                        batterySegments[i].SetActive(false);
                    }
                    break;
                case var _ when percent is > 0.50f and < 0.75f:
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        if (i < 3)
                        {
                            batterySegments[i].SetActive(true);
                            batterySprites[i].color=Color.darkGreen;
                            continue;
                        }
                        batterySegments[i].SetActive(false);
                    }
                    break;
                case var _ when percent is > 0.75f and < 1f:
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        batterySegments[i].SetActive(true);
                        batterySprites[i].color=Color.green;
                    }
                    break;
            }
            
        }
    }
}

