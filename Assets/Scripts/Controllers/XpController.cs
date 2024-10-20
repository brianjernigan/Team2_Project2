using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpController : MonoBehaviour
{
    private const float XpLifespan = 2.5f;
    
    public int XpValue { get; set; }

    private void Start()
    {
        StartCoroutine(XpLifespanRoutine());
    }

    private IEnumerator XpLifespanRoutine()
    {
        yield return new WaitForSeconds(XpLifespan);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(XpLifespanRoutine());
    }
}
