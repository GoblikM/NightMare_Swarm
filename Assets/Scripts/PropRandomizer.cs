using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;


    private void Start()
    {
        SpawnProps();
    }


    // Spawn the props in the spawn points randomly
    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            int randomIndex = Random.Range(0, propPrefabs.Count); // Get a random index
            GameObject prop = Instantiate(propPrefabs[randomIndex], sp.transform.position, Quaternion.identity); // Instantiate the prop
            prop.transform.SetParent(sp.transform); // Set the prop as a child of the spawn point
        }
    }

}
