using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{

    List<GameObject> damagedEnemies;

    protected override void Start()
    {
        base.Start();
        damagedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy") && !damagedEnemies.Contains(collider.gameObject))
        {
            collider.GetComponent<EnemyStats>().TakeDamage(CalculateDamage(out bool isCrit), transform.position, isCrit);
             // Add the enemy to the list of enemies that have been hit,
            // so that they don't take damage again from the same instance of the garlic
            damagedEnemies.Add(collider.gameObject);

        }
        else if (collider.CompareTag("Breakable"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakable) && !damagedEnemies.Contains(collider.gameObject))
            {
                breakable.TakeDamage(CalculateDamage(out bool isCrit), isCrit);
                damagedEnemies.Add(collider.gameObject);
            }
        }
    }



}
