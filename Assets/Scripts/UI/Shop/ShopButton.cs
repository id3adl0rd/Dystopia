using Inventory.Model;
using UnityEngine;

public class ShopButton :  MonoBehaviour
{
    public ItemSO item;
    public ShopUI ui;

    public void Buy()
    {
        ui.BuyItem(item);
    }
}