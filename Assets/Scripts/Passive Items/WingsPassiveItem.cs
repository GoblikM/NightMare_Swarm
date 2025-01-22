using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItem
{

    /// <summary>
    /// Apply the modifier to the player stats
    /// </summary>
    protected override void ApplyModifier()
    {
        // Increase the player's move speed, 1 + the multiplier divided by 100 to get the percentage increase
        // For example, if the multiplier is 10, the player's move speed will be increased by 10%
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }

}
