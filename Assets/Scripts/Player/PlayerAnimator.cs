using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string SPEED = "Speed";

    private Animator animator;
    [SerializeField]
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
    
        animator.SetFloat(HORIZONTAL, player.GetPlayerMoveDirRaw().x);
        animator.SetFloat(VERTICAL, player.GetPlayerMoveDirRaw().y);
        animator.SetFloat(SPEED, player.GetPlayerMoveDirRaw().sqrMagnitude);
    }

}
