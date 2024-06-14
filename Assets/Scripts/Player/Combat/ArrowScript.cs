using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int _damageAmount = 25;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * ArrowVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag != "Player")
            MakeHit(other.collider);
    }

    private void MakeHit(Collider2D collider)
    {
        bool isCriticalHit = Random.Range(0, 100) < 30;
        int dmg = isCriticalHit == true ? _damageAmount * 2 : _damageAmount;
        dmg = (int)(dmg * ArrowVelocity);
        
        Vector2 direction = (collider.transform.position - transform.position).normalized;

        var obj = collider.gameObject; 
        if (obj.GetComponent<EnemyMovement>() || obj.GetComponent<EnemyMovementFollow>())
        {
            var healthController = collider.gameObject.GetComponent<NPCHealthController>();
            healthController.TakeDamage(dmg, direction);

            DamagePopup.Create(collider.gameObject.transform.position, dmg, isCriticalHit);    
        }
        
        Destroy(gameObject);

        /*
        SoundManager.PlaySound(_attackSound);
    */
    }
}
