using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Testing : MonoBehaviour
{
    private void Start()
    {
        //DamagePopup.Create(Vector3.zero, 300);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            bool isCriticalHit = Random.Range(0, 100) < 30;
            DamagePopup.Create(transform.position, 300, isCriticalHit);
            /*var test = new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, 0);
            DamagePopup.Create(test, 300);*/
        }
    }
}
