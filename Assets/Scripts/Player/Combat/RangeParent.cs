using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RangeParent : MonoBehaviour
{
    public SpriteRenderer characterRender, weaponRender, arrowRender;
    public Vector2 PointerPosition { get; set; }

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Slider _bowPowerSlider;
    [SerializeField] private Transform _bow;

    [Range(0, 10)] [SerializeField] private float _bowPower;
    [Range(1, 3)] [SerializeField] private float _maxBowCharge;
    private float bowCharge;
    private bool canFire = true;
    private bool _inCharging;
    
    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        } else if (direction.x > 0)
        {
            scale.y = 1;
        }

        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRender.sortingOrder = characterRender.sortingOrder - 1; 
        }
        else
        {
            weaponRender.sortingOrder = characterRender.sortingOrder + 1; 
        }

        if (_inCharging && canFire)
            ChargeBow();
        else
        {
            if (bowCharge > 0f)
            {
                bowCharge -= 1f * Time.deltaTime;
            }
            else
            {
                bowCharge = 0f;
                canFire = true;
                _inCharging = false;
            }   
            
            _bowPowerSlider.value = bowCharge;
        }
    }

    private void Start()
    {
        _bowPowerSlider.value = 0f;
        _bowPowerSlider.maxValue = _maxBowCharge;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PrepareToChargeBow();
        } else if (context.canceled && canFire)
        {
            FireBow();
        }
    }
    
    private void FireBow()
    {
        if (bowCharge > _maxBowCharge)
            _maxBowCharge = _maxBowCharge;

        float arrowspeed = bowCharge + _bowPower;
        float angle = AngleTowardsMouse(PointerPosition, _bow.position);
        Quaternion rot = Quaternion.Euler(new Vector3(0f, 0f, transform.eulerAngles.z - 90));

        ArrowScript arrow = Instantiate(_arrowPrefab, _bow.position, rot).GetComponent<ArrowScript>();
        arrow.ArrowVelocity = arrowspeed;
        canFire = false;
        _inCharging = false;
        arrowRender.enabled = false;
    }

    private void ChargeBow()
    {
        bowCharge += Time.deltaTime;
        _bowPowerSlider.value = bowCharge;

        if (bowCharge > _maxBowCharge)
        {
            _bowPowerSlider.value = _maxBowCharge;
        }
    }

    private void PrepareToChargeBow()
    {
        arrowRender.enabled = true;
        _inCharging = true;
    }
    
    public float AngleTowardsMouse(Vector3 mousePos, Vector3 pos) {
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(pos);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        return angle;
    }
}