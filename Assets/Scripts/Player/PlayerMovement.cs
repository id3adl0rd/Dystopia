using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private const int WALK_SPEED = 4;
    private const int SPRINT_SPEED = 7;
    
    [SerializeField]
    private ParticleSystem _dust;
    
    [FormerlySerializedAs("_moveSpeed")] [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _sprintSpeed;

    public void SetSprintSpeed(float boost)
    {
        _sprintSpeed = _sprintSpeed * boost;
    }
    
    public void SetWalkSpeed(float boost)
    {
        _walkSpeed = _walkSpeed * boost;
    }
    
    private float _moveSpeed;
    public bool isSprinting { get; private set; }

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private WeaponParent _weaponParent;
    private RangeParent _rangeParent;    
    
    [SerializeField] private GameObject _weaponParentObj;
    [SerializeField] private GameObject _rangeParentObj;

    [SerializeField] private Animator _playerAnimator;

    public PlayerInputControl _playerInput { get; private set; }

    private Player _player;

    private bool IsDusting;

    private GameObject _questprefab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_playerAnimator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _playerInput = new PlayerInputControl();
        _weaponParent = GetComponentInChildren<WeaponParent>();
        _rangeParent = GetComponentInChildren<RangeParent>();
        _questprefab = GameObject.Find("Quest");
        _questprefab.SetActive(false);
    }

    private void Start()
    {
        //OnEnable();
        
        _moveSpeed = _walkSpeed;
    }

    private void OnEnable()
    {
        _playerInput.Player.Sprint.Enable();
        _playerInput.Player.Interact.Enable();
        _playerInput.Player.Inventory.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Sprint.Disable();
        _playerInput.Player.Interact.Disable();
        _playerInput.Player.Inventory.Disable();
    }

    private void Update()
    {
        if (_weaponParentObj.activeSelf)
        {
            _weaponParent.PointerPosition = GetPointerInput();
        }
        
        if (_rangeParentObj.activeSelf)
        {
            _rangeParent.PointerPosition = GetPointerInput();
        }
    }

    private void FixedUpdate()
    {
        //if (DialogueManager.GetInstance()._dialogueIsPlaying)
        //    return;
        
        SetPlayerVelocity();
        //RotateInDirectionOfInput();
    }

    private void SetPlayerVelocity()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.05f);

        _rigidbody.velocity = _smoothedMovementInput * _moveSpeed;
        
        _playerAnimator.SetFloat("moveX", _smoothedMovementInput.x);
        _playerAnimator.SetFloat("moveY", _smoothedMovementInput.y);

        /*
        Debug.Log(_smoothedMovementInput);
        */

        if (_smoothedMovementInput != Vector2.zero)
        {
            _playerAnimator.SetBool("isMoving", true);

            float angle = Mathf.Atan2(_smoothedMovementInput.y, _smoothedMovementInput.x) * Mathf.Rad2Deg;
            _dust.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            _dust.Play();
            IsDusting = true;
        }
        else
        {
            _playerAnimator.SetBool("isMoving", false);
            _dust.Stop();
            IsDusting = false;
        }
    }

    private void RotateInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(rotation);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>().normalized;
    }

    //OnFire => Click
    const float minDist = .9f;
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_player._camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        
        IInteract interact = rayHit.collider.GetComponent<IInteract>() as IInteract;
        if (interact != null)
            interact.OnClick(_player, rayHit.collider.gameObject);

        if (rayHit.collider.name == "NPCShop")
        {
            float dist = Vector2.Distance(rayHit.collider.transform.position, gameObject.transform.position);
        
            if (minDist >= dist)
            {
                GameObject.Find("Shop").GetComponent<ShopUI>().ShowAll();
            }
        }        
        
        if (rayHit.collider.name == "NPC")
        {
            float dist = Vector2.Distance(rayHit.collider.transform.position, gameObject.transform.position);
        
            if (minDist >= dist)
            {
                Debug.Log("open");
                rayHit.collider.GetComponentInChildren<DialogueTrigger>().EnterDialogue();
            }
        }
    }

    public void OnPause()
    {
        if (PauseMenu.Instance.isPaused == true)
            PauseMenu.Instance.ResumeGame();
        else
            PauseMenu.Instance.PauseGame();
        
    }

    public void OnQuest()
    {
        if (_questprefab.activeSelf == true)
            _questprefab.SetActive(false);
        else
        {
            _questprefab.SetActive(true);
            QuestController.instance.SetQuestData();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveSpeed = _sprintSpeed;
            isSprinting = true;
            _player._staminaController.StartConsumingStamina();
        }

        if (context.canceled)
        {
            _moveSpeed = _walkSpeed;
            isSprinting = false;
            _player._staminaController.StartRegen();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_player._camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        
        IInteract interact = rayHit.collider.GetComponent<IInteract>() as IInteract;
        if (interact != null)
            interact.OnClick(_player, rayHit.collider.gameObject);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (_weaponParentObj.activeSelf)
        {
            _weaponParent.Attack();
        }
        
        if (_rangeParentObj.activeSelf)
        {
            _rangeParent.Fire(context);
        }

        //_rangeParent.Fire(context);
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}