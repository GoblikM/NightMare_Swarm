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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !marketEnemies.Contains(collision.gameObject))
        {
            collision.GetComponent<EnemyStats>().TakeDamage(currentDamage);
             // Add the enemy to the list of enemies that have been hit,
            // so that they don't take damage again from the same instance of the garlic
            marketEnemies.Add(collision.gameObject);

        }
    }



}
