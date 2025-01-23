using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickable
{
    public int healAmount;

    public override void Collect()
    {
        if(hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.Heal(healAmount);
        //Destroy(gameObject);

    }


}   

