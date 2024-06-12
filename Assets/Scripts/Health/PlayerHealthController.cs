using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthController : AbstractHealthController, IDamageable
{
    public UnityEvent OnDied, OnDamaged, OnHealthChanged;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioClip sound;
    
    public void TakeDamage(float damageAmount, Vector2 knockback)
    {
        if (_currentHealth == 0)
            return;

        if (IsInvincible)
            return;
        
        _currentHealth -= damageAmount;
        OnHealthChanged.Invoke();
        GameObject.Find("Health Bar").GetComponent<HealthBarUI>().UpdateHealthBar();
        
        transform.position = new Vector2(transform.position.x + knockback.x, transform.position.y + knockback.y);
        
        if (_currentHealth < 0)
            _currentHealth = 0;

        hit.PlayOneShot(sound);
        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
            OnDamaged.Invoke();
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
            return;

        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
        GameObject.Find("Health Bar").GetComponent<HealthBarUI>().UpdateHealthBar();

        if (_currentHealth > _maximumHealth)
            _currentHealth = _maximumHealth;
    }
}