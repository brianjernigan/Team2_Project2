using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private AudioManager _audio;

    private const float Lifespan = 2.5f;
    
    private void Awake()
    {
        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        
        StartCoroutine(FireballLifespan());
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var damage = StatManager.Instance.CurrentDamage;
            var enemyController = other.gameObject.GetComponent<EnemyController>();
            enemyController.DamageEnemy(damage);
            
            _audio.PlayEnemyHitAudio();
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FireballLifespan()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }
}
