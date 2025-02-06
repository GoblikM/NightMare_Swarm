using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCardUI : MonoBehaviour
{

    [Header("Character Stats")]
    [SerializeField]
    private TMP_Text characterName;
    [SerializeField]
    private TMP_Text characterHealth;
    [SerializeField]
    private TMP_Text characterSpeed;
    [SerializeField]
    private TMP_Text characterRecovery;
    [SerializeField]
    private TMP_Text characterMight;
    [SerializeField]
    private TMP_Text characterCritChance;
    [SerializeField]
    private TMP_Text characterProjectileSpeed;
    [SerializeField]
    private TMP_Text characterPickUpRange;
    [SerializeField]
    private Image characterIcon;
    [SerializeField]
    private TMP_Text weaponName;
    [SerializeField]
    private Image weaponIcon;
    [SerializeField]
    private TMP_Text weaponDescription;

    public void SetCharacterData(CharacterSO characterData)
    {
        characterName.text = characterData.Name;
        characterHealth.text = characterData.MaxHealth.ToString();
        characterSpeed.text = characterData.MoveSpeed.ToString();
        characterRecovery.text = characterData.Recovery.ToString();
        characterMight.text = characterData.Might.ToString();
        characterCritChance.text = characterData.CriticalChance.ToString() + " %";
        characterProjectileSpeed.text = characterData.ProjectileSpeed.ToString();
        characterPickUpRange.text = characterData.PickUpRange.ToString();
        characterIcon.sprite = characterData.Icon;
        weaponName.text = characterData.DefaultWeapon.GetComponent<WeaponController>().weaponData.Name;
        weaponIcon.sprite = characterData.DefaultWeapon.GetComponent<WeaponController>().weaponData.Icon;
        weaponDescription.text = characterData.DefaultWeapon.GetComponent<WeaponController>().weaponData.Description;

    }

}
