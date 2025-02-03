using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        FireKnife();
    }

    private void FireKnife()
    {
        Vector3 lastMoveDir = playerMovement.GetPlayerLastMoveDirNormalized();
        for (int i = 0; i < weaponData.NumberOfProjectiles; i++)
        {
            Vector3 direction = (i % 2 == 0) ? lastMoveDir : -lastMoveDir; // Every second knife will be shot in the opposite direction    
            GameObject spawnedKnife = Instantiate(weaponData.WeaponPrefab); // Spawn the knife prefab on the player's position
            spawnedKnife.transform.position = transform.position;
            spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(direction);
        }
    }
}
