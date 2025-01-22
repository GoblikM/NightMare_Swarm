using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    //public static PlayerMovement Instance { get; private set; }


    // References
    private Rigidbody2D rigidBody;
    private PlayerStats player;


    // Movement variables
    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    //[Header("Movement Configuration")]
    //[SerializeField]
    //private float walkSpeed = 5f;

    private void Start()
    {
        // Get the components
        player = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        lastMoveDir = new Vector2(1, 0f); // Default direction is right (1, 0)
    }

    private void Update()
    {
        // Get the input from the player
        HandleInputs();
 
        SetPlayerLastMoveDir();

    }
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        // Move the player
        MovePlayer();
        //Debug.Log("Current Speed: " + (moveDir.normalized * characterData.MoveSpeed).magnitude);

    }

    // funtion to handle inputs, gets the move direction
    private void HandleInputs()
    {
        if (GameManager.instance.isGameOver) return;
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        if(GameManager.instance.isGameOver) return;
        // Move the player
        rigidBody.MovePosition(rigidBody.position + moveDir.normalized * player.CurrentMoveSpeed * Time.fixedDeltaTime);
    }

    private void SetPlayerLastMoveDir()
    {
        if (moveDir.x != 0)
        {
            lastMoveDir = new Vector2(moveDir.x, 0f);
        }
        if (moveDir.y != 0)
        {
            lastMoveDir = new Vector2(0f, moveDir.y);
        }

        // When the player is moving diagonally, set the last move direction to the last move direction
        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMoveDir = new Vector2(moveDir.x, moveDir.y);
        }
    }

    // Get the last direction the player moved
    public Vector2 GetPlayerLastMoveDirNormalized()
    {
        return lastMoveDir.normalized;
    }

    // get move direction raw
    public Vector2 GetPlayerMoveDirRaw()
    {
        return moveDir;
    }

    // get last move direction raw
    public Vector2 GetPlayerLastMoveDirRaw()
    {
        return lastMoveDir;
    }


    // Get the direction the player is moving
    public Vector2 GetPlayerMoveDirNormalized()
    {
        return moveDir.normalized;
    }
}


