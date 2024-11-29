using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{

    List<GameObject> marketEnemies;

    protected override void Start()
    {
        base.Start();
        marketEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy") && !marketEnemies.Contains(collider.gameObject))
        {
            collider.GetComponent<EnemyStats>().TakeDamage(GetCurrentDamage());
             // Add the enemy to the list of enemies that have been hit,
            // so that they don't take damage again from the same instance of the garlic
            marketEnemies.Add(collider.gameObject);

        }
        else if (collider.CompareTag("Breakable"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakable) && !marketEnemies.Contains(collider.gameObject))
            {
                breakable.TakeDamage(GetCurrentDamage());
                marketEnemies.Add(collider.gameObject);
            }
        }
    }



}
