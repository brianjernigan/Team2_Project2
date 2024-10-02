using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyController = other.gameObject.GetComponent<EnemyController>();
            var damage = enemyController.Damage;

            if (!enemyController.IsStunned)
            {
                StatManager.Instance.DamagePlayer(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            StatManager.Instance.IncreaseXp(1);
            Destroy(other.gameObject);
        }
    }
}
