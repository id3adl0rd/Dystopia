using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRender, weaponRender;
    public Vector2 PointerPosition { get; set; }

    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay = 0.3f;
    private bool _attackBlocked;
    [SerializeField] AudioSource swing;
    
    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;
    public int _damageAmount = 10;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }
    
    private void Update()
    {
        if (IsAttacking)
            return;
        
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
    }

    public void Attack()
    {
        if (_attackBlocked)
            return;
        
        swing.Play();
        _animator.SetTrigger("Attack");
        IsAttacking = true;
        _attackBlocked = true;
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_delay);
        _attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    private void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            if (collider.tag == "Player")
                continue;
            
            MakeHit(collider);
        }
    }

    private void MakeHit(Collider2D collider)
    {
        bool isCriticalHit = Random.Range(0, 100) < 30;
        int dmg = isCriticalHit == true ? _damageAmount * 2 : _damageAmount;

        Vector2 direction = (collider.transform.position - transform.position).normalized;
        
        Debug.Log(collider.name);
        var healthController = collider.gameObject.GetComponent<NPCHealthController>();
        healthController.TakeDamage(dmg, direction);
        
        DamagePopup.Create(collider.gameObject.transform.position, dmg, isCriticalHit);
    }
}