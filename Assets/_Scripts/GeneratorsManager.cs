using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts
{
    public class GeneratorsManager : MonoBehaviour
    {
        public static GeneratorsManager Instance; //class singleton
        [SerializeField] private int numberOfGeneratorsToFail;// threshold value
        [ShowNonSerializedField]private int failedGenerators = 0;// currently failed generators, used to compare to fail threshold
        [SerializeField] private List<bool> generators; //stores status of generator (true-working false-failed)
        [SerializeField] private AudioSource alarmSound;
        [SerializeField] GameObject GameOver;
        [SerializeField] Volume volumeComponent;
        public static int taskLength {get; private set;}=2;
        private int ReparedCounter;
        bool alarmLightsEnabeld = false;

        public static void RegisterRepair()
        {
            Instance.ReparedCounter++;
            if (Instance.ReparedCounter >= 4)
            {
                taskLength++;
                Instance.ReparedCounter = 0;
            }
        }
        void Start()
        {
            failedGenerators=0;
        }
        private void Awake()
        {
            //create singleton
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public static int RegisterGenerator()
        {
            return Instance.Register();
        }
        //adds generator to list stores only status and returns id to generator
        private int Register()
        {
            generators.Add(true);
            return generators.Count-1;
        }
        //change state of the generator uses id given to the generator earlier
        public static void ChangeState(int id,bool state)
        {
            if (state)
            {
                Instance.Repair(id);
            }
            else
            {
                Instance.generators[id] = false;
               // Instance.Fail(id);
               Instance.FailGenerator();
            }
        }

        private void Fail(int id)
        {
            generators[id] = false;
            CheckFailedGenerator();
        }

        private void Repair(int id)
        {
            generators[id] = true;
        }
        //Check if required number of generators failed TODO game over
        void CheckFailedGenerator()
        {
            failedGenerators=0;
            foreach (var t in generators.Where(t => !t))
            {
                failedGenerators++;
            }

            if (failedGenerators >= numberOfGeneratorsToFail)
            {
                Time.timeScale = 0;
                GameOver.SetActive(true);
                alarmSound.enabled = false;
            }
        }

        void FailGenerator()
        {
            failedGenerators++;
            if (failedGenerators >= numberOfGeneratorsToFail / 2)
            {
                alarmSound.enabled = true;
                if (!alarmLightsEnabeld)
                {
                    StartCoroutine(LightEffect());
                    alarmLightsEnabeld = true;
                }
            }
            if (failedGenerators >= numberOfGeneratorsToFail)
            {
                Time.timeScale = 0;
                GameOver.SetActive(true);
                alarmSound.enabled = false;
            }
        }

        IEnumerator LightEffect()
        {
            bool reverse = false;
            float currentWeight = 0;
            while (true)
            {
                if(!reverse && currentWeight>= 1) reverse = true;
                if (reverse&&currentWeight <= 0) reverse = false;
                if (reverse)
                {
                    currentWeight -= Time.deltaTime;
                }
                else
                {
                    currentWeight += Time.deltaTime;
                }
                volumeComponent.weight=currentWeight;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}