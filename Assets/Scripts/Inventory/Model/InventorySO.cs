using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItemStruct> _inventoryItems;

        [field: SerializeField] public int _size { get; set; } = 10;
        InventorySO val = null;
        
        public event Action<Dictionary<int, InventoryItemStruct>> OnInventoryUpdated; 

        public void Initialize()
        {
            _inventoryItems = new List<InventoryItemStruct>();

            for (int i = 0; i < _size; i++)
            {
                _inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public void AddItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = new InventoryItemStruct
                    {
                        _item = item,
                        _quantity = quantity
                    };
                    return;
                }
            }
        }

        public void AddItem(InventoryItemStruct item)
        {
            AddItem(item._item, item._quantity);
        }

        public Dictionary<int, InventoryItemStruct> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItemStruct> returnValue = new Dictionary<int, InventoryItemStruct>();

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                    continue;

                returnValue[i] = _inventoryItems[i];
            }

            return returnValue;
        }

        public InventoryItemStruct GetItemAt(int itemIndex)
        {
            return _inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryItemStruct item1 = _inventoryItems[itemIndex1];
            _inventoryItems[itemIndex1] = _inventoryItems[itemIndex2];
            _inventoryItems[itemIndex2] = item1;
            OnItemSwapped();
        }

        private void OnItemSwapped()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItemStruct
    {
        public int _quantity;
        public ItemSO _item;
        public bool IsEmpty => _item == null;

        public InventoryItemStruct ChangeQuantity(int newQuantity)
        {
            return new InventoryItemStruct
            {
                _item = this._item,
                _quantity = newQuantity,
            };
        }

        public static InventoryItemStruct GetEmptyItem() => new InventoryItemStruct
        {
            _item = null,
            _quantity = 0,
        };
    }
}