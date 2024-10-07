using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private GameObject[] _pickupPrefabs;

    private const float TimeToActivate = 3f;

    private float _timeInZone;
    private bool _playerInZone;
    private bool _zoneIsActivated;

    private ParticleSystem _particles;
    private ParticleSystem.MainModule _mainModule;

    private void Awake()
    {
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
        }
    }

    private void Update()
    {
        if (_playerInZone)
        {
            if (!_particles.isPlaying)
            {
                _particles.Play();
            }
            
            _timeInZone += Time.deltaTime;

            if (_timeInZone >= TimeToActivate)
            {
                SpawnPickup();
            }
        }
        else
        {
            if (_particles.isPlaying && !_zoneIsActivated)
            {
                _particles.Stop();
            }
        }
    }

    private void SpawnPickup()
    {
        _zoneIsActivated = true;
        
        if (_particles is not null)
        {
            _mainModule.startColor = Color.green;
        }
        Debug.Log("Spawning pickup!");
    }
}
