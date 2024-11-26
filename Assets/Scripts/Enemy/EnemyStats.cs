using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemySO enemyData;

    // Current stats of the enemy, these will be changed during the game
    private float currentHealth;
    private float currentSpeed;
    private float currentDamage;

    private void Awake()
    {
        // Set the current stats to the default stats
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        currentSpeed = enemyData.MoveSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Destroy the enemy
        Destroy(gameObject);
    }

}
