using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private const float FireballLifespan = 1f;
    public float Damage { get; set; }
    
    private void Awake()
    {
        StartCoroutine(FireballLifespanRoutine());
        Damage = PlayerStatManager.Instance.CurrentDamage;
    }

    private IEnumerator FireballLifespanRoutine()
    {
        yield return new WaitForSeconds(FireballLifespan);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(FireballLifespanRoutine());
    }
}
