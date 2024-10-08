using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneController : MonoBehaviour
{
    private const float TimeToActivate = 3f;

    private float _timeInZone;
    private bool _playerInZone;
    private bool _zoneIsActivated;

    private ParticleSystem _particles;
    private ParticleSystem.MainModule _mainModule;
    private PlayerShootingController _playerShootingController;

    private void Awake()
    {
        _playerShootingController = FindObjectOfType<PlayerShootingController>();
        _particles = GetComponent<ParticleSystem>();
        _mainModule = _particles.main;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInZone = false;
            _timeInZone = 0f;
        }
    }

    private void Update()
    {
        if (!_playerInZone && !_zoneIsActivated)
        {
            if (_particles.isPlaying && !_zoneIsActivated)
            {
                _particles.Stop();
            }

            return;
        }

        _timeInZone += Time.deltaTime;

        if (!_particles.isPlaying)
        {
            _particles.Play();
        }

        if (_timeInZone >= TimeToActivate && !_zoneIsActivated)
        {
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        _zoneIsActivated = true;

        _mainModule.startColor = Color.green;

        var shotTypes = (ShotType[])Enum.GetValues(typeof(ShotType));
        // Exclude default shot type
        var randomShotIndex = Random.Range(1, shotTypes.Length);
        var newShotType = shotTypes[randomShotIndex];

        _playerShootingController?.SetShotType(newShotType);
    }
}
