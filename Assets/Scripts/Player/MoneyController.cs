using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public static MoneyController instance;
    [SerializeField] private int _money;
    
    private void Awake()
    {
        instance = this;

        if (StaticData.money != 0)
        {
            _money = StaticData.money;
        }
    }

    public void AddMoney(int addMoney)
    {
        _money += addMoney;
    }

    public void RemoveMoney(int money)
    {
        _money -= money;
    }

    public int GetMoney()
    {
        return _money;
    }
}
