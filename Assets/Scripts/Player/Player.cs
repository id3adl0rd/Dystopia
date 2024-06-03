using System.Collections;
using Cinemachine;
using Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    public PlayerHealthController _playerHealthController { get; private set; }
    public PlayerMovement _playerMovement { get; private set; }
    public InventoryController _inventoryController { get; private set; }
    public PlayerAmbientController _ambientController { get; private set; }
    public InvincibilityController _invincibilityController { get; private set; }
    public StaminaController _staminaController { get; private set; }
    public FlashEffect _flashEffect { get; private set; }
    public WeaponContoller _weaponContoller { get; private set; }
    public NotifyController _notifyController { get; private set; }
    public LevelController _levelController { get; private set; }
    public ShakeCameraController _shakeCameraController { get; private set; }
    public ClassController _classController { get; private set; }
    
    public Camera _camera { get; private set; }
    private CinemachineVirtualCamera _virtualCamera;

    private GameObject _endUI;
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _inventoryController = GetComponent<InventoryController>();
        _ambientController = GetComponent<PlayerAmbientController>();
        _staminaController = GetComponent<StaminaController>();
        _invincibilityController = GetComponent<InvincibilityController>();
        _flashEffect = GetComponent<FlashEffect>();
        _weaponContoller = GetComponent<WeaponContoller>();
        _notifyController = GetComponent<NotifyController>();
        _levelController = GetComponent<LevelController>();
        _shakeCameraController = GetComponent<ShakeCameraController>();
        _classController = GetComponent<ClassController>();
        _playerHealthController = GetComponent<PlayerHealthController>();
        
        instance = this;
        _camera = Camera.main;
        _virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.Follow = gameObject.transform;

        _endUI = GameObject.Find("GameOver");
        _endUI.SetActive(false);

        _shakeCameraController._vcam = _virtualCamera;
    }

    private void HandleExperienceChange(int newExp)
    {
        _levelController._exp += newExp;

        if (_levelController._exp > 200)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        
    }

    public void OnDead()
    {
        StartCoroutine(EndGameCoroutine());
    }
    
    private IEnumerator EndGameCoroutine()
    {
        _endUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
}