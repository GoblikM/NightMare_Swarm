using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        if(inventoryManager.GetPossibleEvolutions().Count <= 0)
        {
            Debug.LogWarning("No possible evolutions");
            return;
        }
        WeaponEvolutionBlueprint weaponEvolutionBlueprint = inventoryManager.GetPossibleEvolutions()[Random.Range(0, inventoryManager.GetPossibleEvolutions().Count)];
        inventoryManager.EvolveWeapon(weaponEvolutionBlueprint);
    }

}
