using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _healthBarForegroundImage;

    public PlayerHealthController healthController;
    //PlayerHealthController healthController
    public void UpdateHealthBar()
    {
        _healthBarForegroundImage.fillAmount = healthController.RemainingHealthPercentage;
    }
}