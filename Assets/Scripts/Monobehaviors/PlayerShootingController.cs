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

    private bool _canShoot = true;

    public event Action OnAmmoChanged;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _currentShotType = ShotType.Default;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (StatManager.Instance.CurrentAmmo <= 0)
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
            if (StatManager.Instance.CurrentAmmo < StatManager.Instance.MaxAmmo)
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

        StatManager.Instance.CurrentAmmo = StatManager.Instance.MaxAmmo;
        OnAmmoChanged?.Invoke();
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

        fireballRb?.AddForce(_muzzlePosition.forward * StatManager.Instance.CurrentShotSpeed, ForceMode.Impulse);

        StatManager.Instance.CurrentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    public void SetShotType(ShotType newShot)
    {
        _currentShotType = newShot;
        Debug.Log(_currentShotType);
    }
}
