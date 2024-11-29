using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemySO enemyData;

    // Current stats of the enemy, these will be changed during the game
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentSpeed;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    private void Awake()
    {
        // Set the current stats to the default stats
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        currentSpeed = enemyData.MoveSpeed;
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }
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
        // Destroy the enemy object
        Destroy(gameObject);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();

    }


    /**
     * Return the enemy to the player's position when the enemy is too far from the player
     */
    void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + enemySpawner.spawnPositions[Random.Range(0, enemySpawner.spawnPositions.Count)].position;
    }

}
