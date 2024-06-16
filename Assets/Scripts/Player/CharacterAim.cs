using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAim : MonoBehaviour
{
    [SerializeField] public PlayerFOV playerFOV;
    [SerializeField] private GameObject player;
    private PlayerInputControl _playerInput;

    private void Start()
    {
        _playerInput = new PlayerInputControl();
    }
    private void FixedUpdate()
    {
        Vector3 targetPosition = GetPointerInput();
        // Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 position = player.transform.position;
        Vector3 lookDirection = (targetPosition - position).normalized;
        playerFOV.SetOrigin(position);
        playerFOV.SetAimDirection(lookDirection);
    }

    private Vector3 GetPointerInput()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
