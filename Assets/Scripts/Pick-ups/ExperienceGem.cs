using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickable
{
    public int experienceGranted;

    /**
     * When the player collects the experience gem, the player will gain experience.
     **/
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

        //Debug.Log("Experience Gem Collected");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        //Destroy(gameObject);

    }


}
