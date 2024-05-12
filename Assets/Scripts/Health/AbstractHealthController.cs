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
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _currentHealth = _maximumHealth;
    }
}