using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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
    protected Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); // Color of the damage feedback
    public float damageFlashDuration = 0.1f; // Duration of the damage feedback
    public float deathFadeTime = 0.5f; // Time taken for the enemy to fade out when killed
    Color originalColor;
    protected SpriteRenderer spriteRenderer;
    protected EnemyMovement enemyMovement;
    protected Animator animator;

    private bool isAttacking = false;
    protected virtual void Awake()
    {
        // Set the current stats to the default stats
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        currentSpeed = enemyData.MoveSpeed;
    }

    protected virtual void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            ReturnEnemy();
        }

        PlayAttackAnimation();
    }

    public void TakeDamage(float damage, Vector2 sourcePosition, bool isCrit = false, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");
        StartCoroutine(DamageFlash()); // Flash the enemy sprite when it takes damage

        if(damage > 0)
        {
            GameManager.GenerateFloatingText(damage.ToString("F2"), transform, isCrit);
        }

        if (knockbackForce > 0)
        {
            Vector2 direction = (Vector2)transform.position - sourcePosition;
            enemyMovement.Knockback(direction.normalized * knockbackForce, knockbackDuration);
        }

      
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Flash the enemy sprite when it takes damage
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

    public void Die()
    {
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        float originalAlpha = spriteRenderer.color.a;

        while (t < deathFadeTime)
        {
            yield return wait;
            t += Time.deltaTime;

            // set the color for this frame, with the alpha value decreasing over time
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,(1 - t / deathFadeTime) * originalAlpha);

        }

        Destroy(gameObject);
    }


    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();

            if (enemyData.EnemyType == EnemyType.Skeleton)
            {
                if (isAttacking)
                {
                    player.TakeDamage(currentDamage);
                }       
            }
            else if (enemyData.EnemyType == EnemyType.Bat)
            {        
                player.TakeDamage(currentDamage);
            }
        }
    }

    private void PlayAttackAnimation()
    {
        if (!animator) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= enemyData.AttackRange)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(AttackCoroutine());  // Po spuštìní animace spustíme zpoždìní
        }
    }

    private IEnumerator AttackCoroutine()
    {
        // Nastavení flagu isAttacking
        isAttacking = true;

        // Poèkej na délku animace útoku (nebo jiný èas, pokud chceš jiný efekt)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Po skonèení animace se útok považuje za dokonèený
        isAttacking = false;
    }

    /// <summary>
    /// When the enemy is destroyed, call the OnEnemyKilled method in the EnemySpawner script
    /// </summary>
    private void OnDestroy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
            enemySpawner.OnEnemyKilled();

    }


    /// <summary>
    /// Return the enemy to a random spawn position when the player is too far away
    /// </summary>
    protected void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + enemySpawner.spawnPositions[Random.Range(0, enemySpawner.spawnPositions.Count)].position;
    }

}
