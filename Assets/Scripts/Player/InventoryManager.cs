using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6); // max 6 weapons
    public int[] weaponLevels = new int[6];
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6); // max 6 passive items
    public int[] passiveItemLevels = new int[6];

    // UI inventory slots
    public List<Image> weaponUISlotImages = new List<Image>(6);
    public List<Image> passiveItemUISlotsImages = new List<Image>(6);

    [System.Serializable]
    public class WeapondUpgrade
    {
        public GameObject initialWeapond;
        public WeaponSO weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItemSO passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeapondUpgrade> weaponUpgradeOptions = new List<WeapondUpgrade>(); // List of weapon upgrade options
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of passive item upgrade options
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); // List of UI for upgrade options present in the scene

    /// <summary>
    /// Add a weapon to the inventory to specified slot
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="weapon"></param>
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlotImages[slotIndex].enabled = true; // enable the image of the weapon
        weaponUISlotImages[slotIndex].sprite = weapon.weaponData.Icon;
    }

    /// <summary>
    /// Add a passive item to the inventory to specified slot
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="passiveItem"></param>
    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlotsImages[slotIndex].enabled = true; // enable the image of the passive item
        passiveItemUISlotsImages[slotIndex].sprite = passiveItem.passiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
    {
        
        if(weaponSlots.Count > slotIndex)
        {
            // Get the weapon from the slot
            WeaponController weapon = weaponSlots[slotIndex];
            if(!weapon.weaponData.NextLevelPrefab)
            {
                Debug.Log("Weapon does not have a next level prefab" + weapon.name);
                return;
            }

            // Instantiate the next level weapon
            GameObject nextLevelWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            // Set the parent to the player
            nextLevelWeapon.transform.SetParent(transform);
            // Add the weapon to the inventory
            AddWeapon(slotIndex, nextLevelWeapon.GetComponent<WeaponController>());
            // Destroy the current weapon
            Destroy(weapon.gameObject);
            // Update the level of the weapon
            weaponLevels[slotIndex] = nextLevelWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            // Get the passive item from the slot
            PassiveItem passiveitem = passiveItemSlots[slotIndex];
            if (!passiveitem.passiveItemData.NextLevelPrefab)
            {
                Debug.Log("Weapon does not have a next level prefab" + passiveitem.name);
                return;
            }
            // Instantiate the next level passive item
            GameObject nextLevelPassiveItem = Instantiate(passiveitem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            // Set the parent to the player
            nextLevelPassiveItem.transform.SetParent(transform);
            // Add the passive item to the inventory
            AddPassiveItem(slotIndex, nextLevelPassiveItem.GetComponent<PassiveItem>());
            // Destroy the current passive item
            Destroy(passiveitem.gameObject);
            // Update the level of the passive item
            passiveItemLevels[slotIndex] = nextLevelPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;
        }
    }
}
