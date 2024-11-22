using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    KnifeController knifeController;

    protected override void Start()
    {
        base.Start();
        knifeController = FindObjectOfType<KnifeController>();
    }

    void Update()
    {
        // Move the knife in the direction it was thrown
        transform.position += direction * knifeController.speed * Time.deltaTime;
    }

}
