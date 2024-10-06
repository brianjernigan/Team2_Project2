using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private AudioManager _audio;

    public ShotType CurrentShotType { get; set; }

    private bool _canShoot = true;

    public event Action OnAmmoChanged;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        CurrentShotType = ShotType.Default;
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
            if (StatManager.Instance.CurrentAmmo < StatManager.Instance.CurrentMaxAmmo)
            {
                Reload();
            }
        }

        CycleShotType();
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

    private void Shoot()
    {
        DetermineShot(CurrentShotType);
        _audio.PlayShotAudio();
    }
    
    private void Reload()
    {
        _audio.PlayReloadAudio();

        StartCoroutine(ReloadRoutine());

        StatManager.Instance.CurrentAmmo = StatManager.Instance.CurrentMaxAmmo;
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

        DecreaseAmmo();
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
        
        DecreaseAmmo();
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
        var fireball = InstantiateFireBall();
        var fireballRb = GetFireballRigidbody(fireball);

        fireballRb?.AddForce(_muzzlePosition.forward * StatManager.Instance.CurrentShotSpeed, ForceMode.Impulse);

        DecreaseAmmo();
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
}
