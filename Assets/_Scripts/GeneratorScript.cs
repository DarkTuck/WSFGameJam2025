using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class GeneratorScript : MonoBehaviour
    {
        int id;
        private Coroutine timeToFail,timeToDestroy,FailBatteryAnimation;
        [SerializeField]Vector2 timeLimit;
        [SerializeField] private float timeToDestroyMultiplayer;
        [SerializeField] private QTEScript qteScript;
        [SerializeField] private GameObject[] batterySegments;
        [SerializeField] private GameObject interactInfo;
        private SpriteRenderer[] batterySprites;
        Inputs inputs;
        private bool readyToRepair,playerInRange,isBlack;
        void Awake()
        {
            inputs = new Inputs();
        }

        void OnEnable()
        {
            inputs.Player.Enable();
            inputs.Player.Interact.started+=StartRepair;
            interactInfo.SetActive(false);
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
            if(!playerInRange||!readyToRepair)return;
            interactInfo.SetActive(false);
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
            timeLimit=new Vector2(timeLimit.x-0.5f, timeLimit.y-0.5f);
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
            if (!other.CompareTag("Player")) return;
            Debug.Log("player entered");
            playerInRange = true;
            if (readyToRepair) interactInfo.SetActive(true);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            playerInRange = false;
            interactInfo.SetActive(false);
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
            timeToDestroy = null;
            timeToDestroy = StartCoroutine(TimeToDestroy());
            //ChangeState(false);
        }
        void ChangeState(bool state)
        {
            GeneratorsManager.ChangeState(id,state);
        }
        void ChangeColorByState(float baseValue,float currentValue)
        {
            float percent = currentValue/baseValue;
            //Debug.Log(percent);
            switch (percent)
            {
                case var _ when percent is <= 0.0f:
                    if(FailBatteryAnimation!=null) break;
                    foreach (var t in batterySegments)
                    {
                        t.SetActive(true);
                        //batterySprites[i].color=Color.black;
                    }
                    FailBatteryAnimation=StartCoroutine(BatteryAnimation());
                    break;
                case var _ when percent is > 0.0f and < 0.25f:
                    if (FailBatteryAnimation != null)
                    {
                        StopCoroutine(FailBatteryAnimation);
                        FailBatteryAnimation=null;
                    }
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
                    if (FailBatteryAnimation != null)
                    {
                        StopCoroutine(FailBatteryAnimation);
                        FailBatteryAnimation=null;
                    }
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        if (i < 2)
                        {
                            batterySegments[i].SetActive(true);
                            batterySprites[i].color=Color.darkOrange;
                            continue;
                        }
                        batterySegments[i].SetActive(false);
                    }
                    break;
                case var _ when percent is > 0.50f and < 0.75f:
                    if (FailBatteryAnimation != null)
                    {
                        StopCoroutine(FailBatteryAnimation);
                        FailBatteryAnimation=null;
                    }
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        if (i < 3)
                        {
                            batterySegments[i].SetActive(true);
                            batterySprites[i].color=Color.yellow;
                            continue;
                        }
                        batterySegments[i].SetActive(false);
                    }
                    break;
                case var _ when percent is > 0.75f and < 1f:
                    if (FailBatteryAnimation != null)
                    {
                        StopCoroutine(FailBatteryAnimation);
                        FailBatteryAnimation=null;
                    }
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                        batterySegments[i].SetActive(true);
                        batterySprites[i].color=Color.green;
                    }
                    break;
            }
            
        }

        IEnumerator BatteryAnimation()
        {
            while (true)
            {
                if (!isBlack)
                {
                    for (int i = 0; i < batterySegments.Length; i++)
                    {
                    //batterySegments[i].SetActive(true);
                    batterySprites[i].color=Color.black;
                    } 
                    isBlack=true;
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
                for (int i = 0; i < batterySegments.Length; i++)
                {
                    //batterySegments[i].SetActive(true);
                    batterySprites[i].color=Color.red;
                }
                isBlack=false;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

