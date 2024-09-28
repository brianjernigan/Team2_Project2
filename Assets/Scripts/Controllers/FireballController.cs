using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(FireballLifespan());
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage();
            Destroy(gameObject);
        }
    }

    private IEnumerator FireballLifespan()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
