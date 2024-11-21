using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string WALKING = "isWalking";

    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerMovement.moveDir != Vector2.zero)
        {
           animator.SetBool("isWalking", true);
           spriteRenderer.flipX = playerMovement.moveDir.x < 0;
        }
        else
        {
            animator.SetBool("isWalking", false);

        }
    }



}
