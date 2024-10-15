using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private const float Lifespan = 1f;
    
    private void Awake()
    {
        StartCoroutine(FireballLifespan());
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
