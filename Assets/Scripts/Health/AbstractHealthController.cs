using UnityEngine;
public abstract class AbstractHealthController : MonoBehaviour
{
    [SerializeField]
    private protected float _currentHealth = 100;

    [SerializeField]
    private protected float _maximumHealth = 100;

    [SerializeField]
    private protected Rigidbody2D _rb;
    
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
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = _maximumHealth;
    }
}