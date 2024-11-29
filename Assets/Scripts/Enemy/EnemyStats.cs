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

    /// <summary>
    /// Deal damage to the player when the enemy collides with the player
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    /// <summary>
    /// When the enemy is destroyed, call the OnEnemyKilled method in the EnemySpawner script
    /// </summary>
    private void OnDestroy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();

    }


    /// <summary>
    /// Return the enemy to a random spawn position when the player is too far away
    /// </summary>
    void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + enemySpawner.spawnPositions[Random.Range(0, enemySpawner.spawnPositions.Count)].position;
    }

}
