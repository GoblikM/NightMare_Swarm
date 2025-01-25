using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_MOVING = "isMoving";
  

    private Animator animator;
    [SerializeField]
    private Player player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool(IS_MOVING, player.GetPlayerMoveDirRaw() != Vector2.zero);
        CheckWalkDirection();

    }
    
    private void CheckWalkDirection()
    {
        if(player.GetPlayerLastMoveDirRaw().x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (player.GetPlayerLastMoveDirRaw().x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void SetAnimatorController(RuntimeAnimatorController animatorController)
    {
        if(!animator) animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
    }



}
