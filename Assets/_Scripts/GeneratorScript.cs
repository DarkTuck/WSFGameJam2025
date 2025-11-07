using UnityEngine;

namespace _Scripts
{
    public class GeneratorScript : MonoBehaviour
    {
        int id;

        void Start()
        {
            id = GeneratorsManager.RegisterGenerator();
        }

        void Fail()
        {
            GeneratorsManager.FailGenerator(id);
        }
    }
}