using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickable : Pickable
{
    public float expandedPickupRange = 100f; // Velký pickup range
    public float duration = 2f; // Doba trvání

    public override void Collect()
    {
        base.Collect();
        ExpandPlayerPickupRange();
        Destroy(gameObject); // Magnet zmizí po sebrání
    }

    void ExpandPlayerPickupRange()
    {
        PlayerCollector collector = FindObjectOfType<PlayerCollector>();
        if (collector == null) return;

        collector.ExpandPickupRange(expandedPickupRange, duration);
    }
}
