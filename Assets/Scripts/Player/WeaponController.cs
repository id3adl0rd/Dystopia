using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform circleOrigin;
    [SerializeField] private float radius;

    private WeaponParent _weaponParent;
    
    private void Awake()
    {
        _weaponParent = GetComponentInChildren<WeaponParent>();
    }

    public void Click()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }
}
