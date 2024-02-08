using System.Collections.Generic;
using Inventory.Model;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPage _inventoryUI;

        [SerializeField] private InventorySO _inventoryData;

        public List<InventoryItemStruct> initialItems = new List<InventoryItemStruct>();
        
    
        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            _inventoryData.Initialize();
            _inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            
            foreach (var item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                
                _inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItemStruct> inventoryState)
        {
            _inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                _inventoryUI.UpdateData(item.Key, item.Value._item.ItemImage, item.Value._quantity);
            }
        }

        private void PrepareUI()
        {
            _inventoryUI.InitializeInventoryUI(_inventoryData._size);

            this._inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            this._inventoryUI.OnSwapItems += HandleSwapItems;
            this._inventoryUI.OnStartDragging += HandleDragging;
            this._inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItemStruct inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
                return;
            }

            ItemSO item = inventoryItem._item;
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }
    
        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            _inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItemStruct inventoryItem = _inventoryData.GetItemAt(itemIndex);
            
            if (inventoryItem.IsEmpty)
                return;

            _inventoryUI.CreateDraggedItem(inventoryItem._item.ItemImage, inventoryItem._quantity);
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        public void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (_inventoryUI.isActiveAndEnabled == false)
                {
                    _inventoryUI.Show();

                    foreach (var item in _inventoryData.GetCurrentInventoryState())
                    {
                        _inventoryUI.UpdateData(item.Key, item.Value._item.ItemImage, item.Value._quantity);
                    }
                }
                else
                {
                    _inventoryUI.Hide();
                }
            }
        }
    }
}