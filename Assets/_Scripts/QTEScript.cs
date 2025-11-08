using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Random = UnityEngine.Random;

namespace _Scripts
{
    /*
     * This script handles Quick Time Events Logic such as:
     * - Generating Events
     * - Generating Required To be Input
     * - Handling that input
     * [TODO]
     * - Time Limit
     * - Logic For Failed QTE
     * - Logic For Succeeded QTE
     */
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
        //Enables Input for QTE
        void StartTask()
        {
            inputs.Enable();
            inputs.Player.Disable();
            inputs.VQTE.Enable();
            inputs.VQTE.Direction.performed+=TaskInput;
        }
        //Temporary only for testing [TODO] delete this when logic will be added and working
        private void Start()
        {
            GenerateTasks();
        }
        void GenerateTasks()
        {
            //Restart and/or Create QTE
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
                //change highlighted step and check if QTE was done
                Debug.Log("Success");
                Display.SuccessCharacter(currentId);
                currentId++;
                Success();
                //if not continue [TODO] this won't work without logic for exiting When Success
                Display.CurrentCharacter(currentId);

            }
            else
            {
                Debug.Log("Fail");
                Fail();
            }
        }
        //TODO
        void Fail()
        {
            currentId = 0;
        }
        //TODO
        void Success()
        {
            if (currentId >= taskLength)
            {
                currentId = 0;
            }
        }
    }
}