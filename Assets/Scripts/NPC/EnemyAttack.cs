using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 20;
    [SerializeField] private float _knockbackForce = 0.1f;
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            var healthController = collision.gameObject.GetComponent<PlayerHealthController>();

            Collider2D collider = collision.collider;
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            //Vector2 knockback = direction * _knockbackForce;
            
            healthController.TakeDamage(_damageAmount, direction);
        }
    }
}