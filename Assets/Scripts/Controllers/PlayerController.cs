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
    [SerializeField] private Animator _animator;
    [SerializeField] private ShotTypeController _shotTypeController;
    [SerializeField] private Rigidbody _rb;

    private const float PlayerRotationSpeed = 720f;
    private const float FireRate = 0.1f;
    private float _timeSinceLastShot;
    private bool _canShoot;
    
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
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _canShoot = true;
    }
    
    private void Update()
    {
        if (PlayerStatManager.Instance.IsDead) return;
        _timeSinceLastShot += Time.deltaTime;
        HandleShooting();
    }

    private void FixedUpdate()
    {
        if (PlayerStatManager.Instance.IsDead) return;
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
            var direction = new Vector3(_moveDirection.x, 0f, _moveDirection.z).normalized;

            //Calculate target angle for rotation
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //Smoothly rotate the player toward the direction of movement
            var angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * PlayerRotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            //Move the player forward in the direction they are facing
            transform.Translate(Vector3.forward * (PlayerStatManager.Instance.CurrentMoveSpeed * Time.deltaTime));

            //all new
        }
    }
    
    private void HandleShooting()
    {
        if (PlayerStatManager.Instance.IsReloading) return;
        
        if (!PlayerStatManager.Instance.HasAmmo() && !PlayerStatManager.Instance.IsReloading)
        {
            HandleEmptyMagazine();
            return;
        }

        if (_shotTypeController.CurrentShotType == ShotType.AutomaticShot)
        {
            HandleAutoFire();
        }
        else
        {
            HandleManualFire();
        }
    }
    
    private void HandleEmptyMagazine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlayEmptyMagAudio();
        }
    }
    
    private void HandleManualFire()
    {
        if (Input.GetMouseButtonDown(0) && _canShoot)
        {
            Shoot();
        }
    }
    
    private void HandleAutoFire()
    {
        if (Input.GetMouseButton(0) && _canShoot && _timeSinceLastShot >= FireRate)
        {
            Shoot();
            _timeSinceLastShot = 0f;
        }
    }
    
    private void Shoot()
    {
        RotateTowardsMouse();
        _shotTypeController.DetermineShot();
        AudioManager.Instance.PlayShotAudio();
        PlayerStatManager.Instance.DecreaseAmmo();
        _animator.SetTrigger("isFiring");
    }
    
    private void RotateTowardsMouse()
    {
        //Get mouse position in world space
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var targetPosition = hit.point;
            var directionToMouse = (targetPosition - transform.position).normalized;
            directionToMouse.y = 0;

            var lookRotation = Quaternion.LookRotation(directionToMouse);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * PlayerRotationSpeed);
        }
    }

    public void SetSpawnPoint()
    {
        var spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint is not null)
        {
            _rb.MovePosition(spawnPoint.transform.position);
        }
    }
}
