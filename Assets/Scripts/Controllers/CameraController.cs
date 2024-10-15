using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    [SerializeField] private GameObject _player;

    private readonly Vector3 _cameraOffset = new(0f, 25f, -10f);

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
        transform.LookAt(_player.transform);
    }
}
