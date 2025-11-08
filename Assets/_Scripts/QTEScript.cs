using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Random = UnityEngine.Random;
using NaughtyAttributes;

namespace _Scripts
{
    /*
     * This script handles Quick Time Events Logic such as:
     * - Generating Events
     * - Generating Required To be Input
     * - Handling that input
     * - Time Limit
     * - Logic For Failed QTE
     * - Logic For Succeeded QTE
     */
    public class QTEScript : MonoBehaviour
    {
        [SerializeField]QTEDisplay Display;
        [SerializeField] private int baseTimeLimit, timeLimitDecrease;
        [SerializeField] GeneratorScript generator;
        [SerializeField] Vector2 minMaxTimeLimit;
        //private int timeLimit = 5; //test value;
        Inputs inputs;
        readonly Vector2[] directions = new []{Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        Vector2[] task;
        private int currentId = 0;
        //int taskLength = 4;
        Coroutine timeLimitCoroutine;
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
            StartTimeLimit();
        }
        //Temporary only for testing [TODO] delete this when logic will be added and working
        private void OnEnable()
        {
            GenerateTasks();
        }

        void StartTimeLimit()
        {
            timeLimitCoroutine ??= StartCoroutine(TimeLimit());
        }

        void StopTimeLimit()
        {
            if (timeLimitCoroutine == null) return;
            StopCoroutine(timeLimitCoroutine);
            timeLimitCoroutine = null;
        }
        IEnumerator TimeLimit()
        {
            yield return new WaitForSeconds(Random.Range(minMaxTimeLimit.x, minMaxTimeLimit.y));
            generator.FailRepair();
        }
        void GenerateTasks()
        {
            //Restart and/or Create QTE
            currentId = 0;
            task = new Vector2[GeneratorsManager.taskLength];
            for (int i = 0; i < GeneratorsManager.taskLength; i++)
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
            //generator.FailRepair();
            Display.Restart();
            //Display.Clear();
        }
        //TODO
        void Success()
        {
            if (currentId >= GeneratorsManager.taskLength)
            {
                StopTimeLimit();
                currentId = 0;
                generator.SuccessRepair();
                Display.Clear();
                //inputs.Disable();
                inputs.Player.Enable();
                inputs.VQTE.Enable();
                inputs.VQTE.Direction.performed-=TaskInput;
            }
        }
    }
}