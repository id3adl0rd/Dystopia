using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryDescription : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            this._itemImage.gameObject.SetActive(false);
            this._title.text = "";
            this._description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            this._itemImage.gameObject.SetActive(true);
            this._itemImage.sprite = sprite;
            this._title.text = itemName;
            this._description.text = itemDescription;
        }
    }
}