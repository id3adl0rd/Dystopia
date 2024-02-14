using System;
using Inventory.Model;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private InventorySO _inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            int reminder = _inventoryData.AddItem(item._inventoryItem, item._quantity);
            if (reminder == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item._quantity = reminder;
            }
        }
    }
}