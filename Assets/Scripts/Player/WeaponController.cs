using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _characterRender;
    [SerializeField] private SpriteRenderer _weaponRender;
    [SerializeField] public Vector2 pointerPosition { private get; set; }
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay = 0.3f;
    private bool attackBlocked = false;
    
    public bool IsAttacking { get; private set; }

    [SerializeField] private Transform circleOrigin;
    [SerializeField] private float radius;
    
    public void Attack()
    {
        if (attackBlocked)
            return;
        
        _animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(_delay);
        attackBlocked = false;
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
            Debug.Log(collider.name);
        }
    }
}
