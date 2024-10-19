using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CandyController : MonoBehaviour
{
    private const float BobHeight = 0.5f;
    private const float BobSpeed = 2f;

    private const float RotationSpeed = 50f;

    private Vector3 _startPosition;
    private Vector3 _rotationAxis;

    private void Start()
    {
        _startPosition = transform.position;

        _rotationAxis = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
    }

    private void Update()
    {
        BobCandy();
        RotateCandy();
    }

    private void RotateCandy()
    {
        transform.Rotate(_rotationAxis, RotationSpeed * Time.deltaTime);
    }

    private void BobCandy()
    {
        var newY = _startPosition.y + Mathf.Sin(Time.time * BobSpeed) * BobHeight;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }
}
