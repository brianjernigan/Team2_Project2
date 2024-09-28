using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using System;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;

    private ShotType _currentShotType;
    
    private const float FireballSpeed = 20f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _currentShotType = ShotType.Default;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(_currentShotType);
        }
    }

    private void Shoot(ShotType currentShot)
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

        fireballRb?.AddForce(_muzzlePosition.forward * FireballSpeed, ForceMode.Impulse);
    }

    public void SetShotType(ShotType newShot)
    {
        _currentShotType = newShot;
        Debug.Log(_currentShotType);
    }
}
