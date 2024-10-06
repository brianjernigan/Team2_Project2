using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private AudioManager _audio;
    private PlayerShootingController _playerShootingController;

    private const float Lifespan = 2.5f;
    
    private void Awake()
    {
        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
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
            _audio.PlayEnemyHitAudio();
            
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
}
