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

        void ChangeState(bool state)
        {
            GeneratorsManager.ChangeState(id,state);
        }
    }
}