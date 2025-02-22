using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character")]
public class CharacterSO : ScriptableObject
{

    // Add runtime animation controller to the character scriptable object to be able to change the animation controller of the player character
    public RuntimeAnimatorController animatorController;

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

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
    private float criticalChance;
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }

    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    [SerializeField]
    private float pickUpRange;
    public float PickUpRange { get => pickUpRange; set => pickUpRange = value; }


}
