using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpController : MonoBehaviour
{
    private const float XpLifespanDuration = 2f;
    
    public int XpValue { get; set; }

    private void Start()
    {
        StartCoroutine(XpLifespan());
    }

    private IEnumerator XpLifespan()
    {
        yield return new WaitForSeconds(XpLifespanDuration);
        Destroy(gameObject);
    }
}
