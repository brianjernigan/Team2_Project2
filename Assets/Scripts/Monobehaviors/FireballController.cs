using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private PlayerShootingController _playerShootingController;

    private const float Lifespan = 2.5f;
    
    private void Awake()
    {
        _playerShootingController = FindObjectOfType<PlayerShootingController>();
        
        StartCoroutine(FireballLifespan());
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Fireball")) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyController = other.gameObject.GetComponent<EnemyController>();

            var damage = GetDamageAmount();   
            
            enemyController.DamageEnemy(damage);
            AudioManager.Instance.PlayEnemyHitAudio();
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

    private float GetDamageAmount()
    {
        if (_playerShootingController.CurrentShotType == ShotType.HeavyShot)
        {
            return StatManager.Instance.CurrentDamage * 2;
        }

        return StatManager.Instance.CurrentDamage;
    }

    private IEnumerator FireballLifespan()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(FireballLifespan());
        _playerShootingController.IsTracking = false;
    }
}
