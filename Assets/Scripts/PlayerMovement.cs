using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    Rigidbody2D rigidBody;

    public Vector2 moveDir;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputManagment();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void InputManagment()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

}
