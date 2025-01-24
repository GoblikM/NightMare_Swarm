using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickable
{
    public int experienceGranted;
    [SerializeField] private AudioClip collectSound;

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
            // play sound effect
            SoundFXManager.instance.PlaySoundFX(collectSound, transform, 1f);
        }

        //Debug.Log("Experience Gem Collected");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        //Destroy(gameObject);

    }


}
