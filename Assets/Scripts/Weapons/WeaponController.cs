using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject weaponPrefab;
    public float damage;
    public float speed;
    public float cooldownDuration;
    public float currentCooldown;
    public int pierceCount; // How many enemies the weapon can pierce through before being destroyed

    protected PlayerMovement playerMovement;

    protected virtual void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        currentCooldown = cooldownDuration;
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = cooldownDuration;
    }

}
