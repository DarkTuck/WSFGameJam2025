using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Inputs inputs;
    private InputAction move;
    [SerializeField] private float moveSpeed=5;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool canMove = true;
    private bool canClimb = false;

    private float xRange = 9.2f;
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

    void Start()
    {
        player = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        MovePlayer();
        KeepPlayerInboud();
    }

    private void MovePlayer()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();


        if (canMove && (moveInput == Vector2.left || moveInput == Vector2.right)) //moving on x-axis and sprite flip 
        {
            player.Translate(moveInput * moveSpeed, Space.Self);

            if (moveInput == Vector2.left)
            {
                sprite.flipX = false;
            }
            if (moveInput == Vector2.right)
            {
                sprite.flipX = true;
            }

            animator.SetBool("isWalking", true); //enable walking animation
        }

        if (canClimb) //climbing mechanic and animation
        {
            if (moveInput == Vector2.up || moveInput == Vector2.down)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                player.Translate(moveInput * moveSpeed, Space.Self);
                animator.SetBool("isClimbing", true);
            }
        } else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("isClimbing", false);
        }

        if (moveInput == Vector2.zero) //disable walking animation
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void KeepPlayerInboud() //dont allow player to leave the area
    {
        if (player.position.x < -xRange)
        {
            player.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (player.position.x > xRange)
        {
            player.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) //checking for ladders and ground
    {
        if (collision.CompareTag("Ladder"))
        {
            canClimb = true;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            canMove = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //checking for ladders and ground
    {
        if (collision.CompareTag("Ladder"))
        {
            canClimb = false;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            canMove = false;
        }
    }
}
