using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _dust;
    
    [FormerlySerializedAs("_moveSpeed")] [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _sprintSpeed;

    private float _moveSpeed;
    public bool isSprinting { get; private set; }
    public bool isInteract { get; private set; }
    public bool isInventory { get; private set; }

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private Animator _playerAnimator;

    public PlayerInputControl _playerInput { get; private set; }

    private Player _player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _playerInput = new PlayerInputControl();
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
    
    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance()._dialogueIsPlaying)
            return;
        
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

        if (_smoothedMovementInput != Vector2.zero)
        {
            _playerAnimator.SetBool("isMoving", true);
            _dust.Play();
        }
        else
        {
            _playerAnimator.SetBool("isMoving", false);
            _dust.Stop();
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
    public void OnFire(InputAction.CallbackContext context)
    {
        
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
        if (context.started)
            isInteract = true;
        else
            isInteract = false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }
}