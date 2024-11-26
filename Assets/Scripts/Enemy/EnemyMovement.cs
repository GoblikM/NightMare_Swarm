using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemySO enemyData;
    Transform Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, Player.position, enemyData.MoveSpeed * Time.deltaTime);
    }
}

