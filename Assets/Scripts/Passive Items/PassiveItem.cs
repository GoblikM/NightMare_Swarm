using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemSO passiveItemData;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

    protected virtual void ApplyModifier()
    {
        // Apply the modifier to the player stats
    }



}


