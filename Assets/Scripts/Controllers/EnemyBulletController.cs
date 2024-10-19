using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float DamageValue { get; set; }

    private const float Lifespan = 1.5f;

    private void Awake()
    {
        StartCoroutine(EnemyBulletLifespan());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("XP") || other.gameObject.CompareTag("EnemyBullet") || other.gameObject.CompareTag("TrickOrTreat")) return;

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy")) return;
    }

    private IEnumerator EnemyBulletLifespan()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }
}
