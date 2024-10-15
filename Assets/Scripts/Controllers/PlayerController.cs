using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] private Camera _mainCamera;
    
    private const float RotationSpeed = 720f;

    private Rigidbody _rb;
    private Vector3 _moveDirection;

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
        
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.z = Input.GetAxisRaw("Vertical");

        _moveDirection = _moveDirection.normalized;

        var isMoving = _moveDirection != Vector3.zero;

        if (!isMoving)
        {
            _rb.velocity = Vector3.zero;
        }
        else
        {
            _rb.velocity = _moveDirection * PlayerStatManagerSingleton.Instance.CurrentMoveSpeed;
        }

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (!groundPlane.Raycast(ray, out var enter)) return;
        var hitPoint = ray.GetPoint(enter);

        var direction = (hitPoint - transform.position).normalized;
        direction.y = 0;

        var targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
}
