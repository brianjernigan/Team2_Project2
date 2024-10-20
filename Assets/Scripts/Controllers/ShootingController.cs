using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private const float FireRate = 0.1f;
    private float _timeSinceLastShot;
    private bool _canShoot;
    private float _rotationSpeed = 720f; //new
    private Animator _animator; //new
    [SerializeField] private Camera _mainCamera;//new

    private ShotTypeController _shotTypeController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _shotTypeController = GetComponent<ShotTypeController>();
        _canShoot = true;
        _animator = GetComponent<Animator>(); //new
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (AmmoManagerSingleton.Instance.IsReloading) return;
        
        if (!AmmoManagerSingleton.Instance.HasAmmo() && !AmmoManagerSingleton.Instance.IsReloading)
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
            AudioManagerSingleton.Instance.PlayEmptyMagAudio();
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
        RotateTowardsMouse();//new
        _shotTypeController.DetermineShot();
        AudioManagerSingleton.Instance.PlayShotAudio();
        AmmoManagerSingleton.Instance.DecreaseAmmo();
        _animator.SetTrigger("isFiring"); //new
    }

    private void RotateTowardsMouse() //new
    {
        //Get mouse posiiton in world space
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 directionToMouse = (targetPosition - transform.position).normalized;
            directionToMouse.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(directionToMouse);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
