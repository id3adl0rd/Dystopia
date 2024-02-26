using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<InventoryItemStruct> _inventoryItems;

        [field: SerializeField] public int _size { get; set; } = 10;
        
        public event Action<Dictionary<int, InventoryItemStruct>> OnInventoryUpdated; 

        public void Initialize()
        {
            _inventoryItems = new List<InventoryItemStruct>();

            for (int i = 0; i < _size; i++)
            {
                _inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (item.IsStackable == false)
            {
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    if (IsInventoryFull())
                        return quantity;

                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                    }
                    OnItemSwapped();
                    return quantity;
                }   
            }

            quantity = AddStackableItem(item, quantity);
            OnItemSwapped();

            return quantity;
        }

        private int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItemStruct newItme = new InventoryItemStruct
            {
                _item = item,
                _quantity = quantity
            };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItme;
                    return i;
                }
            }
            
            return 0;
        }

        private bool IsInventoryFull()
            => _inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                    continue;
                
                if (_inventoryItems[i]._item.ID == item.ID)
                {
                    int possibleCount = _inventoryItems[i]._item.MaxStackSize - _inventoryItems[i]._quantity;

                    if (quantity > possibleCount)
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i]._item.MaxStackSize);

                        quantity -= possibleCount;
                    }
                    else
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i]._quantity + quantity);
                        OnItemSwapped();
                        
                        return 0;
                    }
                }
            }

            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity, item.DefaultParametersList);
            }

            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int newQuantity, List<ItemParameter> itemState = null)
        {
            InventoryItemStruct newItem = new InventoryItemStruct()
            {
                _item = item,
                _quantity = newQuantity,
                _itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)
            };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItem;
                    return newQuantity;
                }
            }

            return 0;
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
            Debug.Log(itemIndex1);
            Debug.Log(itemIndex2);
            InventoryItemStruct item1 = _inventoryItems[itemIndex1];
            _inventoryItems[itemIndex1] = _inventoryItems[itemIndex2];
            _inventoryItems[itemIndex2] = item1;
            OnItemSwapped();
        }

        private void OnItemSwapped()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount, bool droppedToWorld = false)
        {
            if (_inventoryItems.Count > itemIndex)
            {
                if (_inventoryItems[itemIndex].IsEmpty)
                    return;

                int reminder = _inventoryItems[itemIndex]._quantity - amount;
                if (reminder <= 0)
                    _inventoryItems[itemIndex] = InventoryItemStruct.GetEmptyItem();
                else
                    _inventoryItems[itemIndex] = _inventoryItems[itemIndex].ChangeQuantity(reminder);
                
                OnItemSwapped();
            }
        }
    }

    [Serializable]
    public struct InventoryItemStruct
    {
        public int _quantity;
        public ItemSO _item;
        public List<ItemParameter> _itemState;
        public bool IsEmpty => _item == null;

        public InventoryItemStruct ChangeQuantity(int newQuantity)
        {
            return new InventoryItemStruct
            {
                _item = this._item,
                _quantity = newQuantity,
                _itemState = new List<ItemParameter>(this._itemState)
            };
        }

        public static InventoryItemStruct GetEmptyItem() => new InventoryItemStruct
        {
            _item = null,
            _quantity = 0,
            _itemState = new List<ItemParameter>()
        };
    }
}