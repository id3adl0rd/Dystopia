using System.Collections.Generic;
using System.Text;
using Inventory.Model;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPage _inventoryUI;

        [SerializeField] public InventorySO _inventoryData;

        public List<InventoryItemStruct> initialItems = new List<InventoryItemStruct>();

        [SerializeField] private AudioClip _dropClip;
        [SerializeField] private AudioSource _audioSource;

        private Player _player;
        
        private void Start()
        {
            _player = GetComponent<Player>();
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
            string desc = PrepareDescription(inventoryItem);
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, desc);
        }

        private string PrepareDescription(InventoryItemStruct inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem._item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem._itemState.Count; i++)
            {
                sb.Append($"{inventoryItem._itemState[i].itemParameter.ParameterName} " +
                          $": {inventoryItem._itemState[i].value} / " +
                          $"{inventoryItem._item.DefaultParametersList[i].value}");
            }

            return sb.ToString();
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
            InventoryItemStruct inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            
            ItemSO item = inventoryItem._item;
            string desc = PrepareDescription(inventoryItem);
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, desc);

            IItemAction itemAction = inventoryItem._item as IItemAction;
            if (itemAction != null)
            {
                _inventoryUI.ShowItemAction(itemIndex);
                _inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }
            
            IDestroyableItem destroyableItem = inventoryItem._item as IDestroyableItem;
            if (destroyableItem != null)
            {
                _inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem._quantity));
            }
        }

        public void AddItem(Item item, int quantity = 0, List<ItemParameter> itemState = null)
        {
            if (quantity == 0)
                quantity = item._quantity;
            
            int reminder = _inventoryData.AddItem(item._inventoryItem, quantity, itemState);
            if (reminder == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item._quantity = reminder;
            }
        }
        
        private void DropItem(int itemIndex, int inventoryItemQuantity)
        {
            var inventoryItemStruct = _inventoryData.GetCurrentInventoryState()[itemIndex];
            GameObject obj = Instantiate(inventoryItemStruct._item.ItemPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Item>().SetItem(inventoryItemStruct._item, inventoryItemQuantity);

            _inventoryData.RemoveItem(itemIndex, inventoryItemQuantity);
            _inventoryUI.ResetSelection();
            
            //_audioSource.PlayOneShot(_dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItemStruct inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            
            IDestroyableItem destroyableItem = inventoryItem._item as IDestroyableItem;
            if (destroyableItem != null)
            {
                _inventoryData.RemoveItem(itemIndex, 1);
            }
            
            IItemAction itemAction = inventoryItem._item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem._itemState);
                _audioSource.PlayOneShot(itemAction.actionSfx);

                if (_inventoryData.GetItemAt(itemIndex).IsEmpty)
                    _inventoryUI.ResetSelection();
            }
        }

        public void OpenInventory()
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
                CloseInventory();
            }
        }

        public void CloseInventory()
        {
            _inventoryUI.Hide();
        }
    }
}