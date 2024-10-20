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
    
    private const float RotationSpeed = 10f; //prev 720

    private Rigidbody _rb;
    private Animator _animator; //new
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
        _animator = GetComponent<Animator>();
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
            _animator.SetBool("isWalking", false); //new
            _animator.SetFloat("speed", 0); //new
        }
        else
        {
           // _rb.velocity = _moveDirection * PlayerStatManagerSingleton.Instance.CurrentMoveSpeed;
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("speed", 1);

            //calculate the direction based on input
            Vector3 direction = new Vector3(_moveDirection.x, 0f, _moveDirection.z).normalized;

            //Calculate target angle for rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //Smoothly rotate the player toward the direction of movement
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * RotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            //Move the player forward in the direction they are facing
            transform.Translate(Vector3.forward * PlayerStatManagerSingleton.Instance.CurrentMoveSpeed * Time.deltaTime);

            //all new
        }

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        /*
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (!groundPlane.Raycast(ray, out var enter)) return;
        var hitPoint = ray.GetPoint(enter);

        var direction = (hitPoint - transform.position).normalized;
        direction.y = 0;

        var targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        */ //put into Shooting controller
    }
}
