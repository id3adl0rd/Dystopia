using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private float _disappearTimer;
    private Color _textColor;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private float _disappearSpeed = 1.5f;
    private float _moveYSpeed = 3f;
    private Vector3 moveVector;

    private static int sortingOrder;
    
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i._pfDamagePopup, position, quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }
    
    private void Awake()
    {
        _textMeshPro = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        _textMeshPro.SetText(damageAmount.ToString());

        if (!isCriticalHit)
        {
            _textMeshPro.fontSize = 3.5f;
            _textColor = Color.yellow;
        }
        else
        {
            _textMeshPro.fontSize = 4f;
            _textColor = Color.red;
        }

        _textMeshPro.color = _textColor;
        _disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        _textMeshPro.sortingOrder = sortingOrder;
        
        moveVector = new Vector3(0, _moveYSpeed);
    }
    
    private float pulsate(float time, float speed)
    {
        return (math.abs(math.sin(time * speed)));
    }
    
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 1.2f * Time.deltaTime; 

        if (_disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        
        _disappearTimer -= Time.deltaTime;
        if (_disappearTimer < 0)
        {
            _textColor.a -= _disappearSpeed * Time.deltaTime;
            _textMeshPro.color = _textColor; 

            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
