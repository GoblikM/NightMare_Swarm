using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponSO", menuName ="ScriptableObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField]
    private GameObject weaponPrefab;
    public GameObject WeaponPrefab { get => weaponPrefab; private set => weaponPrefab = value; }

    // Base stats for the weapon
    [SerializeField]
    private float damage;
    public float Damage { get => damage; private set => damage = value; }
    [SerializeField]
    private float speed;
    public float Speed { get => speed; private set => speed = value; }
    [SerializeField]
    private float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }
    [SerializeField]
    private int pierceCount; // How many enemies the weapon can pierce through before being destroyed
    public int PierceCount { get => pierceCount; private set => pierceCount = value; }

    [SerializeField]
    private int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    private GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }


}
