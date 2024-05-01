using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 20;
    [SerializeField] private AudioClip _attackSound;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyMovement>())
        {
            var healthController = collision.gameObject.GetComponent<NPCHealthController>();
            //healthController.TakeDamage(_damageAmount);
            

        }
    }
}
