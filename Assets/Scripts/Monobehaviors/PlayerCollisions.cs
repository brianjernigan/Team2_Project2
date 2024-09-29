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
            var damage = other.gameObject.GetComponent<EnemyController>().Damage;
            StatManager.Instance.DamagePlayer(damage);
        }
    }
}
