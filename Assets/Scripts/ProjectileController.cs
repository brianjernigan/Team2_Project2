using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzlePosition;

    private float _bulletSpeed = 20f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
        var bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb?.AddForce(_muzzlePosition.forward * _bulletSpeed, ForceMode.Impulse);
    }
}
