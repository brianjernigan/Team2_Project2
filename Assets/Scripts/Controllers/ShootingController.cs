using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private const float FireRate = 0.1f;
    private float _timeSinceLastShot;
    private bool _canShoot;

    private ShotTypeController _shotTypeController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _shotTypeController = GetComponent<ShotTypeController>();
        _canShoot = true;
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
        _shotTypeController.DetermineShot();
        AudioManagerSingleton.Instance.PlayShotAudio();
        AmmoManagerSingleton.Instance.DecreaseAmmo();
    }
}
