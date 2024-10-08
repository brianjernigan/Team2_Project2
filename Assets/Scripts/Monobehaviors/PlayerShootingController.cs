using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;

    public ShotType CurrentShotType { get; set; }
    
    private const float FireRate = 0.1f;
    private bool _canShoot = true;
    private float _timeSinceLastShot;
    
    public bool IsTracking { get; set; }

    public event Action OnAmmoChanged;
    public event Action<ShotType> OnShotTypeChanged;

    private Coroutine _activePickupCoroutine;

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
        // CycleShotType(); // Testing only

        if (Input.GetKeyDown(KeyCode.L))
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "L1")
            {
                SceneManager.LoadScene("L2");
            } 
            else if (currentSceneName == "L2")
            {
                SceneManager.LoadScene("L3");
            }
            else
            {
                SceneManager.LoadScene("L1");
                
            }
        }
    }

    private void HandleShooting()
    {
        if (StatManager.Instance.CurrentAmmo <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlayEmptyMagAudio();
            }

            return;
        }

        if (CurrentShotType == ShotType.AutomaticShot)
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
        AudioManager.Instance.PlayShotAudio();
        DecreaseAmmo();
    }
    
    private void DetermineShot(ShotType currentShot)
    {
        switch (currentShot)
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
        AudioManager.Instance.PlayReloadAudio();
        StatManager.Instance.CurrentAmmo = StatManager.Instance.CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
    
    private IEnumerator ReloadRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1f);
        _canShoot = true;
    }

    // Testing only
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
            IsTracking = false;
            Destroy(fireball);
            ShootDefault(); 
        }
        else
        {
            IsTracking = true;
            StartCoroutine(TrackEnemy(fireball, target));
        }
    }

    public IEnumerator TrackEnemy(GameObject fireball, Transform target)
    {
        var fireballRb = GetFireballRigidbody(fireball);

        var turnSpeed = 5f;

        while (fireball is not null && target is not null && IsTracking)
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

            fireballRb?.AddForce(direction * StatManager.Instance.CurrentShotSpeed, ForceMode.Impulse);
        }
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
        OnShotTypeChanged?.Invoke(CurrentShotType);
        AudioManager.Instance.PlayWeaponChangeAudio();

        if (newShot == ShotType.Default) return;
        
        if (_activePickupCoroutine is not null)
        {
            StopCoroutine(_activePickupCoroutine);
        }

        _activePickupCoroutine = StartCoroutine(PickupDuration());
    }

    private IEnumerator PickupDuration()
    {
        yield return new WaitForSeconds(StatManager.Instance.CurrentPickupDuration);
        SetShotType(ShotType.Default);
    }
}
