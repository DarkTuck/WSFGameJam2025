using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    public class GeneratorsManager : MonoBehaviour
    {
        public static GeneratorsManager Instance;
        [SerializeField] private int numberOfGeneratorsToFail;// threshold value
        private int failedGenerators = 0;// currently failed generators, used to compare to fail threshold
        [SerializeField] private List<bool> generators; //stores status of generator (true-working false-failed)

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public static int RegisterGenerator()
        {
            return Instance.Register();
        }

        private int Register()
        {
            generators.Add(true);
            return generators.Count-1;
        }

        public static void FailGenerator(int id)
        {
            Instance.Fail(id);
        }

        private void Fail(int id)
        {
            generators[id] = false;
            CheckFailedGenerator();
        }
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
    }
}