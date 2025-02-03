using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponSO weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemSO passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeapondUpgrade> weaponUpgradeOptions = new List<WeapondUpgrade>(); // List of weapon upgrade options
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of passive item upgrade options
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); // List of UI for upgrade options present in the scene

    public List<WeaponEvolutionBlueprint> weaponEvolutions = new List<WeaponEvolutionBlueprint>();

    PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }

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

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
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

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {

        if (weaponSlots.Count > slotIndex)
        {
            // Get the weapon from the slot
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.Log("Weapon does not have a next level prefab" + weapon.name);
                return;
            }

            // Instantiate the next level weapon
            GameObject nextLevelWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            // Set the parent to the player
            nextLevelWeapon.transform.SetParent(transform);
            // Add the weapon to the inventory
            AddWeapon(slotIndex, nextLevelWeapon.GetComponent<WeaponController>());
            // Destroy the current weapon
            Destroy(weapon.gameObject);
            // Update the level of the weapon
            weaponLevels[slotIndex] = nextLevelWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = nextLevelWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
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

            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = nextLevelPassiveItem.GetComponent<PassiveItem>().passiveItemData;

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        // Create a list of available weapon upgrades and passive item upgrades
        List<WeapondUpgrade> availableWeaponUpgrades = new List<WeapondUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;
            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }

            if (upgradeType == 1)
            {
                WeapondUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); // Apply button functionality
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon) // Spawn a new weapon
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;

                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];

                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;


                            if (!newPassiveItem) // if the passive item is not new then level up the passive item in the inventory
                            {

                                if (!chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab) // if the passive item does not have a next level prefab then break
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); // Apply button functionality
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem) // Spawn a new passive item
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    /**
     * * Get all possible evolutions based on the weapons and passive items in the inventory
     * */
    public List<WeaponEvolutionBlueprint> GetPossibleEvolutions()
    {
        List<WeaponEvolutionBlueprint> possibleEvolutions = new List<WeaponEvolutionBlueprint>();

        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon != null)
            {
                foreach (PassiveItem passiveItem in passiveItemSlots)
                {
                    if (passiveItem != null)
                    {
                        foreach (WeaponEvolutionBlueprint evolution in weaponEvolutions)
                        {
                            if (weapon.weaponData.Level >= evolution.baseWeaponData.Level && passiveItem.passiveItemData.Level >= evolution.passiveItemData.Level)
                            {
                                possibleEvolutions.Add(evolution);
                            }
                        }
                    }
                }
            }
        }

        return possibleEvolutions;
    }

    public void EvolveWeapon(WeaponEvolutionBlueprint evolution)
    {
        // Iterate through the weapon slots and passive item slots
        for (int weaponSlotIndex = 0; weaponSlotIndex < weaponSlots.Count; weaponSlotIndex++)
        {
            WeaponController weapon = weaponSlots[weaponSlotIndex];

            if(!weapon) // If the weapon slot is empty then continue
            {
                continue;
            }

            for (int passiveItemSlotIndex = 0; passiveItemSlotIndex < passiveItemSlots.Count; passiveItemSlotIndex++)
            {
                PassiveItem passiveItem = passiveItemSlots[passiveItemSlotIndex];

                if (!passiveItem) // If the passive item slot is empty then continue with the next slot
                {
                    continue;
                }

                if (weapon && passiveItem && weapon.weaponData.Level >= evolution.baseWeaponData.Level && passiveItem.passiveItemData.Level >= evolution.passiveItemData.Level)
                {
                    GameObject evolvedWeapon = Instantiate(evolution.evolvedWeapon, transform.position, Quaternion.identity);
                    WeaponController evolvedWeaponController = evolvedWeapon.GetComponent<WeaponController>();

                    evolvedWeapon.transform.SetParent(transform); // Set the weapon to be a child of the player
                    AddWeapon(weaponSlotIndex, evolvedWeaponController); // Add the evolved weapon to the inventory
                    Destroy(weapon.gameObject); // Destroy the old weapon

                    // Update level and icon
                    weaponLevels[weaponSlotIndex] = evolvedWeaponController.weaponData.Level;
                    weaponUISlotImages[weaponSlotIndex].sprite = evolvedWeaponController.weaponData.Icon;

                    // Update the upgrade options after evolving the weapon (remove the evolved weapon from the upgrade options)
                    weaponUpgradeOptions.RemoveAt(evolvedWeaponController.weaponData.EvolvedUpgradeToRemove);

                    Debug.Log("EVOLVED!");

                    return;
                }
            }
        }

    }
}
