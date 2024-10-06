using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private AudioManager _audio;

    public ShotType CurrentShotType { get; set; }
    
    private const float FireRate = 0.1f;
    private bool _canShoot = true;
    private bool _isAutoFireEnabled;
    private float _timeSinceLastShot;

    public event Action OnAmmoChanged;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        CurrentShotType = ShotType.Default;
    }
    
    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        
        HandleShooting();
        HandleReloading();
        CycleShotType();
    }

    private void HandleShooting()
    {
        if (StatManager.Instance.CurrentAmmo <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _audio.PlayEmptyMagAudio();
            }

            return;
        }

        if (CurrentShotType == ShotType.AutomaticShot || _isAutoFireEnabled)
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
        if (Input.GetMouseButton(0) && _canShoot && _timeSinceLastShot >= FireRate)
        {
            Shoot();
            _timeSinceLastShot = 0f;
        }
    }
    
    private void Shoot()
    {
        DetermineShot(CurrentShotType);
        _audio.PlayShotAudio();
        DecreaseAmmo();
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
            case ShotType.PiercingShot:
                ShootPiercing();
                break;
            case ShotType.SpreadShot:
                ShootSpread();
                break;
            case ShotType.TrackingShot:
                ShootTracking();
                break;
            case ShotType.AutomaticShot:
            case ShotType.Default:
            default:
                ShootDefault();
                break;
        }
    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && StatManager.Instance.CurrentAmmo < StatManager.Instance.CurrentMaxAmmo)
        {
            Reload();
        }
    }

    private void Reload()
    {
        StartCoroutine(ReloadRoutine());
        _audio.PlayReloadAudio();
        StatManager.Instance.CurrentAmmo = StatManager.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
    
    private IEnumerator ReloadRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1f);
        _canShoot = true;
    }
    
    private void CycleShotType()
    {
        var shotTypes = (ShotType[])Enum.GetValues(typeof(ShotType));
        var shotTypeIndex = Array.IndexOf(shotTypes, CurrentShotType);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            shotTypeIndex = (shotTypeIndex + 1) % shotTypes.Length;
            SetShotType(shotTypes[shotTypeIndex]);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            shotTypeIndex = (shotTypeIndex - 1 + shotTypes.Length) % shotTypes.Length;
            SetShotType(shotTypes[shotTypeIndex]);
        }
    }

    private void ShootTracking()
    {
        var trackingRadius = 25f;
        
        var fireball = InstantiateFireBall();
        var target = FindClosestEnemy(_muzzlePosition.position, trackingRadius);

        if (target is null)
        {
            Destroy(fireball);
            ShootDefault(); 
        }
        else
        {
            StartCoroutine(TrackEnemy(fireball, target));
        }
    }

    private IEnumerator TrackEnemy(GameObject fireball, Transform target)
    {
        var fireballRb = GetFireballRigidbody(fireball);

        var turnSpeed = 5f;

        while (fireball is not null && target is not null)
        {
            var direction = (target.position - fireball.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);

            fireball.transform.rotation = Quaternion.Slerp(fireball.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

            if (fireballRb is not null)
            {
                fireballRb.velocity = fireball.transform.forward * StatManager.Instance.CurrentShotSpeed;
            }
            
            yield return null;
        }
    }

    private Transform FindClosestEnemy(Vector3 position, float radius)
    {
        var colliders = Physics.OverlapSphere(position, radius);
        Transform closestEnemy = null;
        var closestDistance = Mathf.Infinity;

        foreach (var enemyColliders in colliders) 
        {
            if (enemyColliders.CompareTag("Enemy"))
            {
                var distanceToEnemy = Vector3.Distance(position, enemyColliders.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyColliders.transform;
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

            fireballRb?.AddForce(direction * StatManager.Instance.CurrentShotSpeed, ForceMode.Impulse);
        }
    }

    private void ShootPiercing()
    {
        throw new NotImplementedException();
    }

    private void ShootHeavy()
    {
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);
        fireball.transform.localScale *= 2;
        
        fireballRb?.AddForce(_muzzlePosition.forward * (StatManager.Instance.CurrentShotSpeed / 2f), ForceMode.Impulse);
    }

    private void ShootFast()
    {
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);
        fireball.transform.localScale *= 0.75f;
        
        fireballRb?.AddForce(_muzzlePosition.forward * (StatManager.Instance.CurrentShotSpeed * 1.5f), ForceMode.Impulse);
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
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);

        fireballRb?.AddForce(_muzzlePosition.forward * StatManager.Instance.CurrentShotSpeed, ForceMode.Impulse);
    }

    private GameObject InstantiateFireBall()
    {
        return Instantiate(_fireballPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
    }

    private Rigidbody GetFireballRigidbody(GameObject fireball)
    {
        return fireball.GetComponent<Rigidbody>();
    }

    private void DecreaseAmmo()
    {
        StatManager.Instance.CurrentAmmo--;
        OnAmmoChanged?.Invoke();
    }

    public void SetShotType(ShotType newShot)
    {
        CurrentShotType = newShot;
        Debug.Log("Current Shot Type: " + CurrentShotType);
    }

    public void EnableAutoFire()
    {
        _isAutoFireEnabled = true;
    }

    public void DisableAutoFire()
    {
        _isAutoFireEnabled = false;
    }
}
