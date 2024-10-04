using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootingController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzleTransform;

    private float _fireRate;
    private float _shotSpeed;

    private EnemyController _enemyController;
    private Transform _playerTransform;
    private bool _isFiring;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        _fireRate = 2.0f;
        _shotSpeed = 2.5f;
    }

    private void Update()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _enemyController.NavMeshAgent.stoppingDistance)
        {
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
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void FireBullet()
    {
        var bullet = Instantiate(_bulletPrefab, _muzzleTransform.position, _bulletPrefab.transform.rotation);
        var bulletRb = bullet.GetComponent<Rigidbody>();
        var bulletController = bullet.GetComponent<EnemyBulletController>();

        bulletController.DamageValue = _enemyController.Damage;
        
        bulletRb?.AddForce(_muzzleTransform.forward * _shotSpeed, ForceMode.Impulse);
    }
}
