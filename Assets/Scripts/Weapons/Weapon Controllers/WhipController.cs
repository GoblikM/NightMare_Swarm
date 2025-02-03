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

        if (weaponData.NumberOfProjectiles == 1)
        {
            // Pokud je jen jeden projektil, použijeme smìr hráèe.
            CreateWhip(playerMovement.GetPlayerLastMoveDirNormalized());
        }
        else if (weaponData.NumberOfProjectiles == 2)
        {
            // Pokud jsou 2, vytvoøíme jeden whip ve smìru hráèe...
            CreateWhip(playerMovement.GetPlayerLastMoveDirNormalized());
            // ... a druhý whip s opaèným smìrem.
            CreateWhip(-playerMovement.GetPlayerLastMoveDirNormalized());
        }
    }

    private void CreateWhip(Vector3 direction)
    {
        // Nastavíme offset – whip se objeví pøed hráèem.
        // Základní offset je 2 jednotky doprava.
        Vector3 offset = new Vector3(2f, 0f, 0f);
        // Pokud je pøedaný smìr záporný (hráè smìøuje doleva nebo pro druhý whip),
        // invertujeme offset, aby se whip objevil na opaèné stranì.
        if (direction.x < 0)
        {
            offset.x = -offset.x;
        }

        GameObject spawnedWhip = Instantiate(weaponData.WeaponPrefab);
        spawnedWhip.transform.position = transform.position + offset;

        // Pøedáme pøedaný smìr do metody DirectionChecker, která nastaví orientaci whipu.
        spawnedWhip.GetComponent<WhipBehaviour>().DirectionChecker(direction);
    }

}

