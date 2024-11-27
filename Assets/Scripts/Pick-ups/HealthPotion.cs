using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickable, ICollectible
{
    public int healAmount;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.Heal(healAmount);
        //Destroy(gameObject);

    }


}   

