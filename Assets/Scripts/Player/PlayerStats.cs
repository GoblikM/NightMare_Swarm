using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterSO characterData;

    // current stats
    private float currentHealth;
    private float currentRecovery;
    private float currentMoveSpeed;
    private float currentMight;
    private float currentProjectileSpeed;

    // Exp and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    // List of level ranges
    public List<LevelRange> levelRanges;

    // Class for a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    private void Start()
    {
        // Set the experience cap to the first level range
        experienceCap = levelRanges[0].experienceCapIncrease;

    }

    private void Awake()
    {
        // Set the current stats to the default values
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
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

    public void Die()
    {
        // Handle player death
        Debug.Log("Player died");
    }
}
