using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 20;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyMovement>())
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();

            healthController.TakeDamage(_damageAmount);
        }
    }
}
