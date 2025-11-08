using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class QTEScript : MonoBehaviour
    {
        [SerializeField]QTEDisplay Display;
        Inputs inputs;
        Vector2[] directions = new []{Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        Vector2[] task;
        private int currentId = 0;
        int taskLength = 4;
        void Awake()
        {
            inputs = new Inputs();
        }

        void StartTask()
        {
            inputs.Enable();
            inputs.Player.Disable();
            inputs.VQTE.Enable();
            inputs.VQTE.Direction.performed+=TaskInput;
        }

        private void Start()
        {
            GenerateTasks();
        }

        void GenerateTasks()
        {
            currentId = 0;
            task = new Vector2[taskLength];
            for (int i = 0; i < taskLength; i++)
            {
                task[i] = directions[Random.Range(0, directions.Length)];
                Debug.Log(task[i]);
            }
            Display.WriteCharacters(task);
            Display.CurrentCharacter(currentId);
            StartTask();
        }
        void TaskInput(InputAction.CallbackContext context)
        {
            if (context.ReadValue<Vector2>() == task[currentId])
            {
                Debug.Log("Success");
                Display.SuccessCharacter(currentId);
                currentId++;
                Display.CurrentCharacter(currentId);
                Success();
            }
            else
            {
                Debug.Log("Fail");
                Fail();
            }
        }

        void Fail()
        {
            currentId = 0;
        }

        void Success()
        {
            if (currentId >= taskLength)
            {
                currentId = 0;
            }
        }
    }
}