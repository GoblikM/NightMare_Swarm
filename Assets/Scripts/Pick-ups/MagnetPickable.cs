using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickable : Pickable
{
    public float expandedPickupRange = 100f; 
    public float duration = 10f; 

    public override void Collect()
    {
        base.Collect();
        ExpandPlayerPickupRange();
        Destroy(gameObject); 
    }

    void ExpandPlayerPickupRange()
    {
        PlayerCollector collector = FindObjectOfType<PlayerCollector>();
        if (collector == null) return;

        collector.ExpandPickupRange(expandedPickupRange, duration);
    }
}
