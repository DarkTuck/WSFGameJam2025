using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Inputs inputs;
    private InputAction move;
    [SerializeField] private float moveSpeed=5;
    private Transform player;
    //TODO laders and locking up/down movement if not on laders
    #region Inputs
    private void Awake()
    {
        inputs = new Inputs();
    }

    private void OnEnable()
    {
        inputs.Enable();
        move = inputs.Player.Move;
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       player=gameObject.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        player.Translate(moveInput *moveSpeed,Space.Self);
    }
}
