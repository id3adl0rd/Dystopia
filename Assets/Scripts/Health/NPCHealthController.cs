using UnityEngine;
using UnityEngine.Events;

public class NPCHealthController : AbstractHealthController, IDamageable
{
    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealthChanged;
    
    [SerializeField]
    public GameObject _spawnerParent;

    [SerializeField] private AudioClip sound; 
    
    public void OnNPCDeath()
    {
        if (_spawnerParent != null && _spawnerParent.GetComponent<EnemySpawnerController>())
        {
            var _spawner = _spawnerParent.GetComponent<EnemySpawnerController>();
            _spawner._enemyList.Remove(gameObject);
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    
    public void TakeDamage(float damageAmount, Vector2 knockback)
    {
        if (_currentHealth == 0)
            return;

        if (IsInvincible)
            return;
        
        _currentHealth -= damageAmount;
        OnHealthChanged.Invoke();
        
        //transform.position = new Vector2(transform.position.x + knockback.x, transform.position.y + knockback.y);

        if (_currentHealth < 0)
            _currentHealth = 0;
        
        AudioSource.PlayClipAtPoint(sound, transform.position);
        
        if (_currentHealth == 0)
        {
            OnDied.Invoke();
            LevelController.instance.AddExperience(100);
            QuestController.instance.OnKillObj(gameObject.name);
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

        if (_currentHealth > _maximumHealth)
            _currentHealth = _maximumHealth;
    }
}