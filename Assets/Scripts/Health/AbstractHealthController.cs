using UnityEngine;
public abstract class AbstractHealthController : MonoBehaviour
{
    [SerializeField]
    private protected float _currentHealth = 100;

    [SerializeField]
    private protected float _maximumHealth = 100;

    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public bool IsInvincible { get; set; }
    
    private void Awake()
    {
        _currentHealth = _maximumHealth;
    }
}