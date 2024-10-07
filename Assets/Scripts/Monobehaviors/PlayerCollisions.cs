using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerShootingController _playerShootingController;

    private void Awake()
    {
        _playerShootingController = GetComponent<PlayerShootingController>();
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyController = other.gameObject.GetComponent<EnemyController>();
            var damage = (int)enemyController.Damage;

            if (!enemyController.IsStunned)
            {
                StatManager.Instance.DamagePlayer(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            var xpController = other.gameObject.GetComponent<XpController>();
            StatManager.Instance.IncreaseXp(xpController.Value);
            // I don't know why this is necessary, but it is...
            other.enabled = false;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            var bulletController = other.GetComponent<EnemyBulletController>();
            
            StatManager.Instance.DamagePlayer(bulletController.DamageValue);
        }

        HandlePickups(other);
    }

    private void HandlePickups(Collider other)
    {
        if (other.gameObject.CompareTag("FastPickup"))
        {
            _playerShootingController.SetShotType(ShotType.FastShot);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("HeavyPickup"))
        {
            _playerShootingController.SetShotType(ShotType.HeavyShot);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("AutoPickup"))
        {
            _playerShootingController.SetShotType(ShotType.AutomaticShot);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("SpreadPickup"))
        {
            _playerShootingController.SetShotType(ShotType.SpreadShot);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("TrackingPickup"))
        {
            _playerShootingController.SetShotType(ShotType.TrackingShot);
            Destroy(other.gameObject);
        }
    }
}
