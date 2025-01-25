using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private CharacterSO characterData;

    [Header("Player Sounds")]
    public AudioClip[] playerHurtSounds;

    // current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentPickUpRange;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            // check if the value has changed
            if (currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null) {
                    GameManager.instance.currentHealthText.text = "Health: " + currentHealth.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            // check if the value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryText.text = "Recovery: " + currentRecovery.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            // check if the value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                //Debug.Log("value changed");
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + currentMoveSpeed.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            // check if the value has changed
            if (currentMight != value)
            {
                currentMight = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightText.text = "Might: " + currentMight.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            // check if the value has changed
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + currentProjectileSpeed.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }


    public float CurrentPickUpRange
    {
        get { return currentPickUpRange; }
        set
        {
            // check if the value has changed
            if (currentPickUpRange != value)
            {
                currentPickUpRange = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentPickUpRangeText.text = "Pick Up Range: " + currentPickUpRange.ToString();
                }
                // Add any additional logic here that needs to be executed whe the value changes

            }
        }
    }
    #endregion

    public ParticleSystem damageEffect;

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

    [Header("UI")]
    public Image healthBar;
    public Image experienceBar;
    public TMP_Text levelText;

    private void Awake()
    {
        characterData = CharacterSelector.GetCharacterData();
        CharacterSelector.instance.DestroySingleton();
        inventory = GetComponent<InventoryManager>();

        // Set the current stats to the default values
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentPickUpRange = characterData.PickUpRange;

        // Spawn the default weapon
        SpawnWeapon(characterData.DefaultWeapon);

        //set runtime animator controller
        GetComponentInChildren<PlayerAnimator>().SetAnimatorController(characterData.animatorController);

    }

    private void Start()
    {
        // Set the experience cap to the first level range
        experienceCap = levelRanges[0].experienceCapIncrease;

        // Set the current stats display in the UI
        GameManager.instance.currentHealthText.text = "Health: " + CurrentHealth.ToString();
        GameManager.instance.currentRecoveryText.text = "Recovery: " + CurrentRecovery.ToString();
        GameManager.instance.currentMoveSpeedText.text = "Move Speed: " + CurrentMoveSpeed.ToString();
        GameManager.instance.currentMightText.text = "Might: " + CurrentMight.ToString();
        GameManager.instance.currentProjectileSpeedText.text = "Projectile Speed: " + CurrentProjectileSpeed.ToString();
        GameManager.instance.currentPickUpRangeText.text = "Pick Up Range: " + CurrentPickUpRange.ToString();

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateLevelText();


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
        CheckLvlUp();

        UpdateExperienceBar();
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
        UpdateLevelText();
        GameManager.instance.StartLevelUp();
    }

    void UpdateExperienceBar()
    {
        experienceBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }

    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            CurrentHealth -= damage;
            SoundFXManager.instance.PlaySoundFX(playerHurtSounds[Random.Range(0, playerHurtSounds.Length)], transform, 1f);

            if (damageEffect)
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
            }

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            Debug.Log("Player took damage");
            if (CurrentHealth <= 0)
            {
                Die();
            }
            
            UpdateHealthBar();
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if(CurrentHealth > characterData.MaxHealth)
        {
            CurrentHealth = characterData.MaxHealth;
        }

        UpdateHealthBar();
    }

    public void RecoverHealth()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
      
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void Die()
    {
        // Handle player death
        Debug.Log("Player died");
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlotImages, inventory.passiveItemUISlotsImages);
            GameManager.instance.GameOver();
        }
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
