using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    public class GeneratorScript : MonoBehaviour
    {
        int id;
        private Coroutine timeToFail,timeToDestroy;
        [SerializeField]int timeLimit;
        [SerializeField] private QTEScript qteScript;
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
        }

        public void FailRepair()
        {
            ChangeState(false);
        }
        IEnumerator TimeToFail()
        {
            yield return new WaitForSeconds(timeLimit);
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
            yield return new WaitForSeconds(timeLimit*2);
            FailRepair();
            //ChangeState(false);
        }
        void ChangeState(bool state)
        {
            GeneratorsManager.ChangeState(id,state);
        }
    }
}