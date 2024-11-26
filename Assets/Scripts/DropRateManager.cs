using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    // OnDestroy is called when the object with this script is destroyed
    private void OnDestroy()
    {
        float randomNumber = Random.Range(0f, 100f);
        // List with possible drops
        List<Drops> possibleDrops = new List<Drops>();

        // Check if the random number is less than the drop rate of the item
        foreach (Drops drop in drops)
        {
            // If the random number is less than the drop rate, add the item to the possible drops list
            if (randomNumber <= drop.dropRate)
            {
                possibleDrops.Add(drop);
            }
        }
        // If there are possible drops, instantiate a random one
        if (possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }

    }


}
