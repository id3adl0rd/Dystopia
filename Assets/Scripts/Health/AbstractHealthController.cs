using UnityEngine;
public abstract class AbstractHealthController : MonoBehaviour
{
    [SerializeField]
    private protected float _currentHealth = 100;

    [SerializeField]
    private protected float _maximumHealth = 100;

    private protected Player _player;
    
    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public bool IsInvincible { get; set; }

    public void SetHealth(float health)
    {
        _currentHealth = health;
    }
    
    public void SetMaxHealth(float health)
    {
        _maximumHealth = health;
    }

    public float GetHealth()
    {
        return _currentHealth;
    }
    
    public float GetMaxHealth()
    {
        return _maximumHealth;
    }
    
    private void Awake()
    {
        _player = GetComponent<Player>();

        _currentHealth = _maximumHealth;
    }
}