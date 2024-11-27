using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemy;
    Transform Player;

    private void Start()
    {
        enemy = GetComponent<EnemyStats>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, Player.position, enemy.currentSpeed * Time.deltaTime);
    }
}

