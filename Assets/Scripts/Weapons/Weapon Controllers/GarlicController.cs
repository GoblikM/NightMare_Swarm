using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicController : WeaponController
{
    protected override void Start()
    {
        base.Start();

    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedGarlic = Instantiate(weaponPrefab);
        spawnedGarlic.transform.position = transform.position; // Spawn the garlic at the player's position
        spawnedGarlic.transform.parent = transform; // Set the player as the parent of the garlic


    }
}
