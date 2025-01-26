using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipController : WeaponController
{

    protected override void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();

        // Ur�ete offset pozice
        Vector3 offset = new Vector3(2f, 0f, 0f); // Nap��klad 1.5 jednotky doprava
        if (playerMovement.GetPlayerLastMoveDirNormalized().x < 0)
        {
            offset.x = -offset.x; // Pokud hr�� kouk� doleva, posuneme bi� doleva
        }

        // Vytvo�en� efektu bi�e s offsetem
        GameObject spawnedWhip = Instantiate(weaponData.WeaponPrefab);
        spawnedWhip.transform.position = transform.position + offset;
        spawnedWhip.GetComponent<WhipBehaviour>().DirectionChecker(playerMovement.GetPlayerLastMoveDirNormalized());
    }

}

