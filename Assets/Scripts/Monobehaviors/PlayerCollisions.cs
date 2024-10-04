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
            var damage = (int)enemyController.Damage;

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
            var xpController = other.gameObject.GetComponent<XpController>();
            StatManager.Instance.IncreaseXp(xpController.Value);
            // I don't know why this is necessary, but it is...
            other.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            var bulletController = other.GetComponent<EnemyBulletController>();
            
            StatManager.Instance.DamagePlayer(bulletController.DamageValue);
        }
    }
}
