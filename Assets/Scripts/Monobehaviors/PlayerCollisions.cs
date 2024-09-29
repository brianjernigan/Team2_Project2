using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision other)
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
}
