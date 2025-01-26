using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] 
    private float speed = 10f;
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float lifeTime = 2f;

    private EnemyStats enemyStats;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        enemyStats = FindObjectOfType<EnemyStats>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(enemyStats.currentDamage, "Projectile"); // Deal damage to the player
            }
            Destroy(gameObject); // Destroy the projectile
        }

    }
}
