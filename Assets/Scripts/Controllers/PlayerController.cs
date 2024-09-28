using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private Camera _mainCamera;
    
    private float _moveSpeed = 10f;
    private float _rotationSpeed = 720f;

    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Awake()
    {
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
            _rb.velocity = _moveDirection * _moveSpeed;
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

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage()
    {
        Debug.Log("Player takes damage");
    }
}
