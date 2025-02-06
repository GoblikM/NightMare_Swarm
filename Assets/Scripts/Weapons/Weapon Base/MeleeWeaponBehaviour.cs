using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponSO weaponData;
    public float destroyAfterSeconds;

    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    private void Awake()
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

    public float CalculateDamage(out bool isCrit)
    {
        
        float baseDamage = currentDamage * PlayerStats.instance.CurrentMight;
        float critChance = PlayerStats.instance.CurrentCriticalChance;  
        float critMultiplier = 2.0f;

        isCrit = Random.value < (critChance / 100f);
        return isCrit ? baseDamage * critMultiplier : baseDamage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyStats>().TakeDamage(CalculateDamage(out bool isCrit), transform.position, isCrit);
        }
        else if (collider.CompareTag("Breakable"))
        {
            if(collider.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(CalculateDamage(out bool isCrit), isCrit);
                
            }
        }
    }


}
