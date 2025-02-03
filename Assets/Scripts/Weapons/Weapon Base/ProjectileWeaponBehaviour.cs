using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponSO weaponData;


    protected Vector3 direction;
    public float destroyAfterSeconds;


    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;


    protected virtual void Awake()
    {
        // Set the current stats to the default stats
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.PierceCount;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }


    /// <summary>
    /// Get the current damage of the weapon after applying the player's might
    /// </summary>
    public float GetCurrentDamage()
    {
        return currentDamage * FindObjectOfType<PlayerStats>().CurrentMight;
    }

    public void DirectionChecker(Vector3 direction)
    {
        this.direction = direction;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.localEulerAngles;

        if(dirX < 0 && dirY == 0) // left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if(dirX == 0 && dirY < 0) // down
        {
            scale.y = scale.y * -1;
        }
        else if(dirX == 0 && dirY > 0) // up
        {
            scale.x = scale.x * -1;
        }
        else if(direction.x > 0 && direction.y > 0) // right up
        {
            rotation.z = 0f;
        }
        else if(direction.x > 0 && direction.y < 0) // right down
        {
            rotation.z = -90f;
        }
        else if(direction.x < 0 && direction.y > 0) // left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        } 
        else if(direction.x < 0 && direction.y <0) // left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }
  
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);
            if (weaponData.Name != "Whip")
            {
                ReducePierce();
            }
        }
        // Check if the projectile hits a breakable object
        else if (collider.CompareTag("Breakable"))
        {
            if(collider.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                // Damage the breakable object
                breakable.TakeDamage(GetCurrentDamage());
                if(weaponData.Name != "Whip")
                {
                    ReducePierce();
                }
            }
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

}
