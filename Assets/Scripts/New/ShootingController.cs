using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private float _fireRate = 0.1f;
    private float _timeSinceLastShot;
    private bool _canShoot;

    private ShotTypeManager _shotTypeManager;

    private void Start()
    {
        _shotTypeManager = GetComponent<ShotTypeManager>();
        _canShoot = true;
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (!AmmoManager.Instance.HasAmmo()) return;

        if (_shotTypeManager.CurrentShotType == ShotType.AutomaticShot)
        {
            HandleAutoFire();
        }
        else
        {
            HandleManualFire();
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
        if (Input.GetMouseButton(0) && _canShoot && _timeSinceLastShot >= _fireRate)
        {
            Shoot();
            _timeSinceLastShot = 0f;
        }
    }

    private void Shoot()
    {
        _shotTypeManager.DetermineShot();
        AudioManager.Instance.PlayShotAudio();
        AmmoManager.Instance.DecreaseAmmo();
    }
}
