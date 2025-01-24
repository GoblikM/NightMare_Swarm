using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "ScriptableObjects/Weapon Evolution Blueprint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    public WeaponSO baseWeaponData;
    public PassiveItemSO passiveItemData;
    public WeaponSO evolvedWeaponData;
    public GameObject evolvedWeapon;

}
