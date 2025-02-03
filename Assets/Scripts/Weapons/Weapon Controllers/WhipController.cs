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
            // Pokud je jen jeden projektil, pou�ijeme sm�r hr��e.
            CreateWhip(playerMovement.GetPlayerLastMoveDirNormalized());
        }
        else if (weaponData.NumberOfProjectiles == 2)
        {
            // Pokud jsou 2, vytvo��me jeden whip ve sm�ru hr��e...
            CreateWhip(playerMovement.GetPlayerLastMoveDirNormalized());
            // ... a druh� whip s opa�n�m sm�rem.
            CreateWhip(-playerMovement.GetPlayerLastMoveDirNormalized());
        }
    }

    private void CreateWhip(Vector3 direction)
    {
        // Nastav�me offset � whip se objev� p�ed hr��em.
        // Z�kladn� offset je 2 jednotky doprava.
        Vector3 offset = new Vector3(2f, 0f, 0f);
        // Pokud je p�edan� sm�r z�porn� (hr�� sm��uje doleva nebo pro druh� whip),
        // invertujeme offset, aby se whip objevil na opa�n� stran�.
        if (direction.x < 0)
        {
            offset.x = -offset.x;
        }

        GameObject spawnedWhip = Instantiate(weaponData.WeaponPrefab);
        spawnedWhip.transform.position = transform.position + offset;

        // P�ed�me p�edan� sm�r do metody DirectionChecker, kter� nastav� orientaci whipu.
        spawnedWhip.GetComponent<WhipBehaviour>().DirectionChecker(direction);
    }

}

