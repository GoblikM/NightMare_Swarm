using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // Move the knife in the direction it was thrown
        transform.position += direction * weaponData.Speed * Time.deltaTime;
    }

}
