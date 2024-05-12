using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    private Player _player;
    
    [SerializeField]
    private float _currentStamina = 100;

    [SerializeField]
    private float _maximumStamina = 100;

    [SerializeField] public float _restoreAmount = 1f;
    [SerializeField] public float _consumeAmount = 1f;
    
    private IEnumerator _regenCoroutine, _consumeCoroutine;
    
    public float RemainingStaminaPercentage
    {
        get
        {
            return _currentStamina / _maximumStamina;
        }
    }
    
    private void Awake()
    {
        _currentStamina = _maximumStamina;
        _player = GetComponent<Player>();
    }

    public bool CanAffordStaminaForAction(float value)
    {
        return _currentStamina >= value;
    }

    public void ConsumeStamina(float amount = 1f)
    {
        StopRegen();
        
        if (_currentStamina - amount < 0)
        {
            _currentStamina = 0;
            return;
        }

        _currentStamina -= amount;
    }

    public void RestoreStamina(float amount = 1f)
    {
        if (_currentStamina + amount > _maximumStamina)
        {
            _currentStamina = _maximumStamina;
            return;
        }
            
        _currentStamina += amount;
    }

    private IEnumerator StaminaRegen()
    {
        while (true)
        {
            if (_player._playerMovement.isSprinting == true)
            {
                StopRegen();
                yield break;
            }
            
            if (_currentStamina >= _maximumStamina)
            {
                _currentStamina = _maximumStamina;
                StopRegen();
                yield break;
            }

            if (_currentStamina > _maximumStamina / 2)
            {
                if (_player._playerMovement._playerInput.Player.Sprint.enabled == false)
                    _player._playerMovement._playerInput.Player.Sprint.Enable();
            }
            
            yield return new WaitForSeconds(0.5f);
            RestoreStamina(_restoreAmount);
        }
    }
    
    private IEnumerator StaminaConsuming()
    {
        while(true){
            if (_player._playerMovement.isSprinting == false)
            {
                StopConsuming();
                yield break;
            }
            
            yield return new WaitForSeconds(0.5f);

            ConsumeStamina(_consumeAmount);
            
            if (_currentStamina <= 0)
            {
                if (_player._playerMovement.isSprinting == true)
                {
                    _player._playerMovement._playerInput.Player.Sprint.Disable();
                    //_player._playerMovement._playerInput.Player.Sprint.performed => false;
                }
                
                yield break;
            }
        }

    }

    public void StopConsuming()
    {
        if (_consumeCoroutine != null)
        {
            StopCoroutine(_consumeCoroutine);
            _consumeCoroutine = null;
        }
    }

    public void StopRegen()
    {
        if (_regenCoroutine != null)
        {
            StopCoroutine(_regenCoroutine);
            _regenCoroutine = null;
        }
    }

    public void StartConsumingStamina()
    {
        StopRegen();
        StopConsuming();
        
        if (_consumeCoroutine == null)
        {
            IEnumerator coroutine = StaminaConsuming();
            _consumeCoroutine = coroutine;
            StartCoroutine(coroutine);
        }
    }

    public void StartRegen()
    {
        StopRegen();
        StopConsuming();
        
        if (_regenCoroutine == null)
        {
            IEnumerator coroutine = StaminaRegen();
            _regenCoroutine = coroutine;
            StartCoroutine(coroutine);
        }
    }

    public void Update()
    {
    }
}