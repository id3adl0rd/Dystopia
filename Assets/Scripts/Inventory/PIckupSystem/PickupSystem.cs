using System;
using Inventory.Model;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private InventorySO _inventoryData;
    private Player _player;
    private Item _item;
    public bool _canPickUp { get; private set; }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canPickUp = true;
        _item = collision.GetComponent<Item>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canPickUp = false;
        _item = null;
    }
    
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_player._camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        if (!_canPickUp) return;
        
        if (_item != null)
        {
            int reminder = _inventoryData.AddItem(_item._inventoryItem, _item._quantity);
            if (reminder == 0)
            {
                _item.DestroyItem();
            }
            else
            {
                _item._quantity = reminder;
            }

            _item = null;
        }
    }
}