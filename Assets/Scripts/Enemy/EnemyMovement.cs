using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    private void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            MoveTowardsPlayer();
        }

    }

    private void MoveTowardsPlayer()
    {
        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the enemy away from the player
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="duration"></param>
    public void Knockback(Vector2 velocity, float duration)
    {
        // If the enemy is already being knocked back, don't apply another knockback
        if (knockbackDuration > 0) return;
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }


}

