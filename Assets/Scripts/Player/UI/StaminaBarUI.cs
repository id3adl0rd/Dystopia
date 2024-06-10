using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _staminaBarForegroundImage;

    public StaminaController _staminaController;
    //PlayerHealthController healthController
    public void UpdateStaminaBar()
    {
        _staminaBarForegroundImage.fillAmount = _staminaController.RemainingStaminaPercentage;
    }
}