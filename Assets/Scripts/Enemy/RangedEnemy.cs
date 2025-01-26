using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyStats
{
    [Header("Projectile Settings")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float attackCooldown = 2f;
    private float attackTimer = 0f;
    [SerializeField]
    private float projectileSpeed = 10f;

    private bool canAttack = true;

    Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
       if(Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < enemyData.AttackRange)
        {
            enemyMovement.StopMoving();
            if (canAttack)
            {
                Attack();
            }
        }
        else
        {
            enemyMovement.ResumeMoving();
        }

    }

    private void Attack()
    {
        if ( attackTimer <= 0f)
        {
            if(animator != null)
            {
                animator.SetTrigger("Attack");
            }

            //ShootProjectile();
            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void ShootProjectile()
    {
        // Create a new projectile on the enemy's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // Set projectile's parent to the enemy
        projectile.transform.SetParent(transform);
        // Get the direction to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Set the velocity of the projectile, get rb of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

    }

}
