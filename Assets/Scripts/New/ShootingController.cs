using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private const float FireRate = 0.1f;
    private float _timeSinceLastShot;
    private bool _canShoot;

    private ShotTypeManager _shotTypeManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
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
        if (!AmmoManager.Instance.HasAmmo())
        {
            HandleEmptyMagazine();
            return;
        }

        if (_shotTypeManager.CurrentShotType == ShotType.AutomaticShot)
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
        _shotTypeManager.DetermineShot();
        AudioManager.Instance.PlayShotAudio();
        AmmoManager.Instance.DecreaseAmmo();
    }
}
