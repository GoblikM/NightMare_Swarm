using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // constants for the animator parameters
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string SPEED = "Speed";

    private Rigidbody2D rigidBody;
    private Animator animator;

    private Vector2 moveDir;
    private Vector2 lastMoveDir;



    [Header("Movement Configuration")]
    [SerializeField]
    private float walkSpeed = 5f;


    private void Awake()
    {
        // Get the components
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        lastMoveDir = new Vector2(1, 0f); // Default direction is right (1, 0)
    }



    private void Update()
    {
        // Get the input from the player
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");

        // Set the animator parameters
        animator.SetFloat(HORIZONTAL, moveDir.x);
        animator.SetFloat(VERTICAL, moveDir.y);
        animator.SetFloat(SPEED, moveDir.sqrMagnitude);

        if(moveDir.x != 0)
        {
            lastMoveDir = new Vector2(moveDir.x, 0f);
        }
        if(moveDir.y != 0)
        {
            lastMoveDir = new Vector2(0f, moveDir.y);
        }

        // When the player is moving diagonally, set the last move direction to the last move direction
        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMoveDir = new Vector2(moveDir.x, moveDir.y);
        }
       
       

    }

    public Vector2 GetLastMoveDirNormalized()
    {
        return lastMoveDir;
    }


    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        // Move the player
        rigidBody.MovePosition(rigidBody.position + moveDir.normalized * walkSpeed * Time.fixedDeltaTime);
        Debug.Log("Current Speed: " + (moveDir.normalized * walkSpeed).magnitude);

    }


    // Get the direction the player is moving
    public Vector2 GetPlayerMoveDirNormalized()
    {
        return moveDir;
    }
}


