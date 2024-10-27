using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    [SerializeField] private GameObject _player;

    private readonly Vector3 _cameraOffset = new(0f, 10f, -6.5f);
    private readonly Quaternion _cameraRotation = Quaternion.Euler(50f, 0f, 0f);

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transform.position = _player.transform.position + _cameraOffset;
    }
    
    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        var targetPosition = _player.transform.position + _cameraOffset;

        transform.position = targetPosition;
        transform.rotation = _cameraRotation;
    }
}
