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
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        //shopItemTemplate.gameObject.SetActive(false);
    }

    [SerializeField] private ItemSO _item;
    private void Start()
    {
        CreateItemButton(_item.ItemImage, _item.Name, 10, 0);
        CreateItemButton(_item.ItemImage, _item.Name, 10, 1);
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
        _inv._inventoryData.AddItemAlt(_item, 1);
    }
}
