using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventoryManager;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Sprite openChestSprite;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            // change the sprite to open chest
            spriteRenderer.sprite = openChestSprite;
            //Destroy(gameObject);
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
