using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthController : AbstractHealthController, IDamageable
{
    public UnityEvent OnDied, OnDamaged, OnHealthChanged;
    
    public void TakeDamage(float damageAmount, Vector2 knockback)
    {
        if (_currentHealth == 0)
            return;

        if (IsInvincible)
            return;
        
        _currentHealth -= damageAmount;
        OnHealthChanged.Invoke();
        
        transform.position = new Vector2(transform.position.x + knockback.x, transform.position.y + knockback.y);
        _player._notifyController.AddToQueue("blya", 1f);
        
        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();
        else
            OnDamaged.Invoke();
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
            return;

        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();

        if (_currentHealth > _maximumHealth)
            _currentHealth = _maximumHealth;
    }
}