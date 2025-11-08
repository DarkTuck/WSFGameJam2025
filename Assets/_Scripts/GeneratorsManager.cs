using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class GeneratorsManager : MonoBehaviour
    {
        public static GeneratorsManager Instance; //class singleton
        [SerializeField] private int numberOfGeneratorsToFail;// threshold value
        private int failedGenerators = 0;// currently failed generators, used to compare to fail threshold
        [SerializeField] private List<bool> generators; //stores status of generator (true-working false-failed)

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
                Debug.Log("Game Over");
            }
        }

        void FailGenerator()
        {
            failedGenerators++;
            if (failedGenerators >= numberOfGeneratorsToFail)
            {
                Debug.Log("Game Over");
            }
        }
    }
}