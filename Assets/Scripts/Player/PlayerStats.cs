using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterSO characterData;

    // current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentPickUpRange;


    // Exp and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    // Class for a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    // List of level ranges
    public List<LevelRange> levelRanges;

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    InventoryManager inventory;
    public int weaponSlotIndex;
    public int passiveItemSlotIndex;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    private void Awake()
    {
        characterData = CharacterSelector.GetCharacterData();
        CharacterSelector.instance.DestroySingleton();
        inventory = GetComponent<InventoryManager>();

        // Set the current stats to the default values
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentPickUpRange = characterData.PickUpRange;

        // Spawn the default weapon
        SpawnWeapon(characterData.DefaultWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
    }

    private void Start()
    {
        // Set the experience cap to the first level range
        experienceCap = levelRanges[0].experienceCapIncrease;

    }


    private void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        RecoverHealth();

    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
    }

    public void CheckLvlUp()
    {
        if(experience >= experienceCap)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        experience -= experienceCap;
        int experienceCapIncrease = 0;
        foreach(LevelRange range in levelRanges)
        {
            if(level >= range.startLevel && level <= range.endLevel)
            {
                experienceCapIncrease = range.experienceCapIncrease;
                break;
            }
        }
        experienceCap += experienceCapIncrease;
    }

    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            currentHealth -= damage;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            Debug.Log("Player took damage");
            if (currentHealth <= 0)
            {
                Die();
            }     
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if(currentHealth > characterData.MaxHealth)
        {
            currentHealth = characterData.MaxHealth;
        }
    }

    public void RecoverHealth()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
      
    }

    public void Die()
    {
        // Handle player death
        Debug.Log("Player died");
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponSlotIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.Log("Cannot add more weapons");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        // Add the weapon to the inventory
        inventory.AddWeapon(weaponSlotIndex, spawnedWeapon.GetComponent<WeaponController>());
        // increase the weapon slot index
        weaponSlotIndex++;
    }


    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemSlotIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.Log("Cannot add more passive Items!");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemSlotIndex, spawnedPassiveItem.GetComponent<PassiveItem>());
        passiveItemSlotIndex++;
    }
}
