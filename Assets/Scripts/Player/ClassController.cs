using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ClassController : MonoBehaviour
{
    public static ClassController instance { get; private set; }
    [SerializeField] private ClassSO _class;
    [SerializeField] private GameObject _weaponMelee;
    [SerializeField] private GameObject _weaponRange;

    public void SetClass(ClassSO pl_class)
    {
        _class = pl_class;
    }
    
    private void Awake()
    {
        instance = this;
        SetClass(StaticData.classSO);

        if (_class.GetUID() == "bow")
        {
            _weaponMelee.SetActive(false);
            _weaponRange.SetActive(true);
        }
        else
        {
            _weaponMelee.SetActive(true);
            _weaponRange.SetActive(false);
        }
    }

    private void Start()
    {
        Player.instance._playerHealthController.SetMaxHealth(math.round(Player.instance._playerHealthController.GetMaxHealth() *
                                                                        GetHPBoost()));
        Player.instance._playerHealthController.SetHealth(Player.instance._playerHealthController.GetMaxHealth());
        Player.instance._playerMovement.SetSprintSpeed(GetSpeedBoost());
        Player.instance._playerMovement.SetWalkSpeed(GetSpeedBoost());
    }

    public string GetClassID()
    {
        return _class.GetUID();
    }

    public string GetClassName()
    {
        return _class.GetName();
    }
    
    public float GetHPBoost()
    {
        return _class.GetHPBoost();
    }
    
    public float GetSpeedBoost()
    {
        return _class.GetSpeedBoost();
    }
}
