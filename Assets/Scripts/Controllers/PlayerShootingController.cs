using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private AudioManager _audio;

    private ShotType _currentShotType;

    private float _currentAmmo;

    private bool _canShoot = true;

    public event Action<float> OnAmmoChanged;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _currentShotType = ShotType.Default;

        _currentAmmo = StatManager.Instance.CurrentAmmo;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentAmmo <= 0)
            {
                _audio.PlayEmptyMagAudio();
                return;
            }

            if (_canShoot)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_currentAmmo < StatManager.Instance.CurrentAmmo)
            {
                Reload();
            }
        }
    }

    private void Shoot()
    {
        DetermineShot(_currentShotType);
        _audio.PlayShotAudio();
    }
    
    private void Reload()
    {
        _audio.PlayReloadAudio();

        StartCoroutine(ReloadRoutine());
        
        _currentAmmo = StatManager.Instance.CurrentAmmo;
        OnAmmoChanged?.Invoke(_currentAmmo);
    }

    private IEnumerator ReloadRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1f);
        _canShoot = true;
    }

    private void DetermineShot(ShotType currentShot)
    {
        switch (currentShot)
        {
            case ShotType.BouncingShot:
                ShootBounce();
                break;
            case ShotType.CircleShot:
                ShootCircle();
                break;
            case ShotType.ExplodingShot:
                ShootExplode();
                break;
            case ShotType.FastShot:
                ShootFast();
                break;
            case ShotType.HeavyShot:
                ShootHeavy();
                break;
            case ShotType.LargeShot:
                ShootLarge();
                break;
            case ShotType.PiercingShot:
                ShootPiercing();
                break;
            case ShotType.SpreadShot:
                ShootSpread();
                break;
            case ShotType.TrackingShot:
                ShootTracking();
                break;
            case ShotType.Default:
            default:
                ShootDefault();
                break;
        }
    }

    private void ShootTracking()
    {
        throw new NotImplementedException();
    }

    private void ShootSpread()
    {
        throw new NotImplementedException();
    }

    private void ShootPiercing()
    {
        throw new NotImplementedException();
    }

    private void ShootLarge()
    {
        throw new NotImplementedException();
    }

    private void ShootHeavy()
    {
        throw new NotImplementedException();
    }

    private void ShootFast()
    {
        throw new NotImplementedException();
    }

    private void ShootExplode()
    {
        throw new NotImplementedException();
    }

    private void ShootCircle()
    {
        throw new NotImplementedException();
    }

    private void ShootBounce()
    {
        throw new NotImplementedException();
    }

    private void ShootDefault()
    {
        var fireball = Instantiate(_fireballPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
        var fireballRb = fireball.GetComponent<Rigidbody>();

        fireballRb?.AddForce(_muzzlePosition.forward * StatManager.Instance.BaseShotSpeed, ForceMode.Impulse);

        OnAmmoChanged?.Invoke(--_currentAmmo);
    }

    public void SetShotType(ShotType newShot)
    {
        _currentShotType = newShot;
        Debug.Log(_currentShotType);
    }
}
