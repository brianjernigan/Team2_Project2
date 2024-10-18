using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private const float Lifespan = 1f;
    public float Damage { get; set; }
    
    private void Awake()
    {
        StartCoroutine(FireballLifespan());
        Damage = PlayerStatManagerSingleton.Instance.CurrentDamage;
    }

    private IEnumerator FireballLifespan()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(FireballLifespan());
    }
}
