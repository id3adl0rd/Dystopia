using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField] private InventoryItem _itemPrefab;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private InventoryDescription _itemDescription;
        [SerializeField] private MouseFollower _mouseFollower;
    
        List<InventoryItem> listOfItems = new List<InventoryItem>();

        private void Awake()
        {
            Hide();
            _mouseFollower.Toggle(false);
            _itemDescription.ResetDescription();
        }

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private int _currentrlyDraggedItemIndex = -1;

        public void InitializeInventoryUI(int invSize)
        {
            for (int i = 0; i < invSize; i++)
            {
                InventoryItem item = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_contentPanel);
                item.transform.localScale = new Vector3(1, 1, 1);

                item.OnItemClicked += HandleItemSelection;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnRightMouseBtnClick += HandleShowItemActions;
            
                listOfItems.Add(item);
            }
        }

        public void UpdateData(int itemIndex, Sprite sprite, int itemQuantity)
        {
            if (listOfItems.Count > itemIndex)
            {
                listOfItems[itemIndex].SetData(sprite, itemQuantity);
            }
        }
    
        private void HandleShowItemActions(InventoryItem InventoryItemUI)
        {

        }

        private void HandleEndDrag(InventoryItem InventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(InventoryItem InventoryItemUI)
        {
            int index = listOfItems.IndexOf(InventoryItemUI);
            if (index == -1)
            {
                return;
            }
        
            OnSwapItems?.Invoke(_currentrlyDraggedItemIndex, index);
            HandleItemSelection(InventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            _mouseFollower.Toggle(false);
            _currentrlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(InventoryItem InventoryItemUI)
        {
            int index = listOfItems.IndexOf(InventoryItemUI);
            if (index == -1)
                return;

            _currentrlyDraggedItemIndex = index;
            HandleItemSelection(InventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            _mouseFollower.Toggle(true);
            _mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(InventoryItem InventoryItemUI)
        {
            int index = listOfItems.IndexOf(InventoryItemUI);
            if (index == -1)
                return;
        
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _itemDescription.ResetDescription();
            ResetSelection();
        }

        public void ResetSelection()
        {
            _itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (var item in listOfItems)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string itemDescription)
        {
            _itemDescription.SetDescription(itemImage, itemName, itemDescription);
            DeselectAllItems();
            listOfItems[itemIndex].Select();
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}