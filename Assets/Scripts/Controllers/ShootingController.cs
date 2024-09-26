using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;

    private ShotType _currentShotType;

    private void Start()
    {
        _currentShotType = ShotType.Default;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        throw new System.NotImplementedException();
    }

    private void ShootSpread()
    {
        throw new System.NotImplementedException();
    }

    private void ShootPiercing()
    {
        throw new System.NotImplementedException();
    }

    private void ShootLarge()
    {
        throw new System.NotImplementedException();
    }

    private void ShootHeavy()
    {
        throw new System.NotImplementedException();
    }

    private void ShootFast()
    {
        throw new System.NotImplementedException();
    }

    private void ShootExplode()
    {
        throw new System.NotImplementedException();
    }

    private void ShootCircle()
    {
        throw new System.NotImplementedException();
    }

    private void ShootBounce()
    {
        throw new System.NotImplementedException();
    }

    private void ShootDefault()
    {
        const float fireballSpeed = 20f;

        var fireball = Instantiate(_fireballPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
        var fireballRb = fireball.GetComponent<Rigidbody>();

        fireballRb?.AddForce(_muzzlePosition.forward * fireballSpeed, ForceMode.Impulse);
    }

    public void SetShotType(ShotType newShot)
    {
        _currentShotType = newShot;
    }
}