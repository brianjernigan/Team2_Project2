using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzleTransform;

    private const float FireRate = 2.0f;
    private const float ShotSpeed = 2.5f;
    private const float AimRotationSpeed = 45f;
    
    private Transform _playerTransform;
    private bool _isFiring;
    

    private void Update()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        
        var playerIsWithinDistance = Vector3.Distance(transform.position, _playerTransform.position) <=
                                     12.5f;
        
        if (playerIsWithinDistance)
        {
            RotateTowardsPlayer();
            
            if (!_isFiring)
            {
                StartFiring();
            }
        }
        else
        {
            if (_isFiring)
            {
                StopFiring();
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        var directionToPlayer = (_playerTransform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        var targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.rotation =
            Quaternion.Slerp(transform.rotation, targetRotation, AimRotationSpeed * Time.deltaTime);
    }

    private void StartFiring()
    {
        _isFiring = true;
        StartCoroutine(FireAtPlayer());
    }
    
    private void StopFiring()
    {
        _isFiring = false;
        StopCoroutine(FireAtPlayer());
    }

    private IEnumerator FireAtPlayer()
    {
        while (_isFiring)
        {
            FireBullet();
            yield return new WaitForSeconds(FireRate);
        }
    }

    private void FireBullet()
    {
        var bullet = Instantiate(_bulletPrefab, _muzzleTransform.position, _bulletPrefab.transform.rotation);
        var bulletRb = bullet.GetComponent<Rigidbody>();
        var bulletController = bullet.GetComponent<EnemyBulletController>();

        bulletController.DamageValue = GetComponent<EnemyController>().Damage;
        
        bulletRb?.AddForce(_muzzleTransform.forward * ShotSpeed, ForceMode.Impulse);
    }
}
