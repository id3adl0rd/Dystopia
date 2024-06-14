using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private Image _borderImage;

        public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool _empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        private void FixedUpdate()
        {
            if (_itemImage.gameObject == null)
            {
                Destroy(this);
            }
        }

        public void ResetData()
        {
            Debug.Log(_itemImage);
            if (_itemImage == null) 
                Destroy(_itemImage);
            
            _itemImage.gameObject.SetActive(false);
            _empty = true;
        }
        public void Deselect()
        {
            this._borderImage.enabled = false;
        }
        public void SetData(Sprite sprite, int quantity)
        {
            _itemImage.gameObject.SetActive(true);
            _itemImage.sprite = sprite;
            _quantityText.text = quantity + "";
            _empty = false;
        }

        public void Select()
        {
            _borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_empty)
                return;
        
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}