using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private InventoryController _inv;
    private Transform container;
    [SerializeField] private Transform shopItemTemplate;
    [SerializeField] private Transform closeButton;
    //[SerializeField] private GameObject containerObj;

    private void Awake()
    {
        container = transform.Find("container");
        
        CreateItemButton(_item.ItemImage, _item.Name, 10, 0);
        container.gameObject.SetActive(false);
        //shopItemTemplate = container.Find("shopItemTemplate");
        //shopItemTemplate.gameObject.SetActive(false);
    }

    [SerializeField] private ItemSO _item;
    private void Start()
    {
    }

    public void ShowAll()
    {
        container.gameObject.SetActive(true);
        RectTransform buttonItemRectTransform = closeButton.GetComponent<RectTransform>();
        buttonItemRectTransform.anchoredPosition = new Vector2(0, -80 * 2);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTansform = shopItemTransform.GetComponent<RectTransform>();
        float shopItemHeight = 80f;
        shopItemRectTansform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("price").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
    }
    
    public void BuyItem(ItemSO _item)
    {
        if (MoneyController.instance.GetMoney() > 0)
        {
            _inv._inventoryData.AddItemAlt(_item, 1);
            MoneyController.instance.RemoveMoney(10);
        }
    }

    public void Close()
    {
        container.gameObject.SetActive(false);
    }
}
