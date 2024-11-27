using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character")]
public class CharacterSO : ScriptableObject
{

    // Base stats for the player character, this will be used to set the stats of the player character
    [SerializeField]
    private GameObject defaultWeapon;
    public GameObject DefaultWeapon { get => defaultWeapon; set => defaultWeapon = value; }

    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField]
    private float recovery;
    public float Recovery { get => recovery; set => recovery = value; }

    [SerializeField]
    private float might;
    public float Might { get => might; set => might = value; }
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    [SerializeField]
    private float pickUpRange;
    public float PickUpRange { get => pickUpRange; set => pickUpRange = value; }


}
