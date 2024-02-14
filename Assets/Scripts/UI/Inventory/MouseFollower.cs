using System;
using System.Collections;
using System.Collections.Generic;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private InventoryItem _item;

    public void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _camera = Camera.main;
        _item = GetComponentInChildren<InventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity)
    {
        _item.SetData(sprite, quantity);
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, Mouse.current.position.ReadValue(),
            _canvas.worldCamera, out position);

        transform.position = _canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
