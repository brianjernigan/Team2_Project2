using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneController : MonoBehaviour
{
    private const float TimeToActivate = 5f;

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

    public void EnterZone()
    {
        if (_zoneIsActivated) return;
        _timeInZone = 0f;
        StartCoroutine(ActivateZoneRoutine());
    }

    public void ExitZone()
    {
        StopAllCoroutines();
        _timeInZone = 0f;

        if (_particles.isPlaying && !_zoneIsActivated)
        {
            _particles.Stop();
        }
    }

    private IEnumerator ActivateZoneRoutine()
    {
        if (!_particles.isPlaying)
        {
            _particles.Play();
        }

        while (_timeInZone < TimeToActivate)
        {
            _timeInZone += Time.deltaTime;
            yield return null;
        }

        if (!_zoneIsActivated)
        {
            ActivateZone();
        }
    }

    private void ActivateZone()
    {
        _zoneIsActivated = true;
        _mainModule.startColor = Color.green;
        LevelManager.Instance.RegisterHouseVisited();
    }
}
