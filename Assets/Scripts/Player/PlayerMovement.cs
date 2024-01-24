using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("_moveSpeed")] [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _sprintSpeed;

    private float _moveSpeed;
    private bool isSprinting;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private Animator _playerAnimator;

    private PlayerInputControl _playerInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerInput = new PlayerInputControl();
        OnEnable();
        _playerInput.Player.Sprint.performed += ctx => SprintPressed();
        _playerInput.Player.Sprint.canceled += ctx => SprintReleased();
        _moveSpeed = _walkSpeed;
    }

    private void OnEnable()
    {
        //_playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        //RotateInDirectionOfInput();
    }

    private void SprintPressed()
    {
        isSprinting = true;
        _moveSpeed = _sprintSpeed;
    }

    private void SprintReleased()
    {
        isSprinting = false;
        _moveSpeed = _walkSpeed;
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
        }
        else
        {
            _playerAnimator.SetBool("isMoving", false);
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

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}