using Inventory;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHealthController _playerHealthController { get; private set; }
    public PlayerMovement _playerMovement { get; private set; }
    public InventoryController _inventoryController { get; private set; }
    public PlayerAmbientController _ambientController { get; private set; }
    public InvincibilityController _invincibilityController { get; private set; }
    public StaminaController _staminaController { get; private set; }
    public FlashEffect _flashEffect { get; private set; }
    public WeaponContoller _weaponContoller { get; private set; }
    public NotifyController _notifyController { get; private set; }
    
    public Camera _camera { get; private set; }
    
    private void Awake()
    {
        _playerHealthController = GetComponent<PlayerHealthController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _inventoryController = GetComponent<InventoryController>();
        _ambientController = GetComponent<PlayerAmbientController>();
        _staminaController = GetComponent<StaminaController>();
        _invincibilityController = GetComponent<InvincibilityController>();
        _flashEffect = GetComponent<FlashEffect>();
        _weaponContoller = GetComponent<WeaponContoller>();
        _notifyController = GetComponent<NotifyController>();
        _camera = Camera.main;
    }
}