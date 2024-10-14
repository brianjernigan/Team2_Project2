using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTypeController : MonoBehaviour
{
    public ShotType CurrentShotType { get; private set; } = ShotType.Default;

    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;

    private bool _isTracking;
    
    public event Action<ShotType> OnShotTypeChanged;

    public void SetShotType(ShotType newShot)
    {
        CurrentShotType = newShot;
        OnShotTypeChanged?.Invoke(CurrentShotType);
        AudioManager.Instance.PlayWeaponChangeAudio();

        if (newShot == ShotType.Default) return;

        StartCoroutine(PickupDuration());
    }

    public void DetermineShot()
    {
        switch (CurrentShotType)
        {
            case ShotType.FastShot:
                ShootFast();
                break;
            case ShotType.HeavyShot:
                ShootHeavy();
                break;
            case ShotType.SpreadShot:
                ShootSpread();
                break;
            case ShotType.TrackingShot:
                ShootTracking();
                break;
            case ShotType.Default:
            case ShotType.AutomaticShot:
            default:
                ShootDefault();
                break;
        }
    }
    
    private void ShootTracking()
    {
        var trackingRadius = 25f;
        
        var fireball = InstantiateFireBall();
        var target = FindClosestEnemy(_muzzlePosition.position, trackingRadius);

        if (target is null)
        {
            _isTracking = false;
            Destroy(fireball);
            ShootDefault(); 
        }
        else
        {
            _isTracking = true;
            StartCoroutine(TrackEnemy(fireball, target));
        }
    }

    public IEnumerator TrackEnemy(GameObject fireball, Transform target)
    {
        var fireballRb = GetFireballRigidbody(fireball);

        var turnSpeed = 5f;

        while (fireball is not null && target is not null && _isTracking)
        {
            var direction = (target.position - fireball.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);

            fireball.transform.rotation = Quaternion.Slerp(fireball.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

            if (fireballRb is not null)
            {
                fireballRb.velocity = fireball.transform.forward * PlayerStats.Instance.CurrentShotSpeed;
            }
            
            yield return null;
        }
    }

    private Transform FindClosestEnemy(Vector3 position, float radius)
    {
        const int maxColliders = 50;
        
        var hitColliders = new Collider[maxColliders];
        var numColliders = Physics.OverlapSphereNonAlloc(position, radius, hitColliders);
        
        Transform closestEnemy = null;
        var closestDistance = Mathf.Infinity;

        for (var i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                var distanceToEnemy = Vector3.Distance(position, hitColliders[i].transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hitColliders[i].transform;
                }
            }
        }

        return closestEnemy;
    }

    private void ShootSpread()
    {
        var numProjectiles = 3;
        var spreadAngle = 15f;

        for (var i = 0; i < numProjectiles; i++)
        {
            var fireball = InstantiateFireBall();
            var fireballRb = GetFireballRigidbody(fireball);

            var angleOffset = (i - (numProjectiles - 1) / 2f) * spreadAngle;
            var direction = Quaternion.Euler(0, angleOffset, 0) * _muzzlePosition.forward;

            fireballRb?.AddForce(direction * PlayerStats.Instance.CurrentShotSpeed, ForceMode.Impulse);
        }
    }

    private void ShootHeavy()
    {
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);
        fireball.transform.localScale *= 2;
        
        fireballRb?.AddForce(_muzzlePosition.forward * (PlayerStats.Instance.CurrentShotSpeed / 2f), ForceMode.Impulse);
    }

    private void ShootFast()
    {
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);
        fireball.transform.localScale *= 0.75f;
        
        fireballRb?.AddForce(_muzzlePosition.forward * (PlayerStats.Instance.CurrentShotSpeed * 1.5f), ForceMode.Impulse);
    }

    private void ShootDefault()
    {
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);

        fireballRb?.AddForce(_muzzlePosition.forward * PlayerStats.Instance.CurrentShotSpeed, ForceMode.Impulse);
    }
    
    private GameObject InstantiateFireBall()
    {
        return Instantiate(_fireballPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
    }

    private Rigidbody GetFireballRigidbody(GameObject fireball)
    {
        return fireball.GetComponent<Rigidbody>();
    }

    private IEnumerator PickupDuration()
    {
        yield return new WaitForSeconds(10f);
        SetShotType(ShotType.Default);
    }
}
