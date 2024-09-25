using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private Vector3 _cameraOffset = new(0f, 15f, -10f);

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
