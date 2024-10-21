using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DoorbellZoneController : MonoBehaviour
{
    private const float TimeToActivate = 1.5f;

    private float _timeHeldDown;
    private bool _isInZone;
    private bool _bellIsRung;
    private bool _isTextScaled;

    [SerializeField] private Animator _fenceAnim;
    [SerializeField] private HouseSpawner _houseSpawner;
    [SerializeField] private ParticleSystem _particles;
    
    private RingDoorbellUIController _ringDoorbellUIController;

    private void Awake()
    {
        _ringDoorbellUIController = GameObject.FindWithTag("RingPanel").GetComponent<RingDoorbellUIController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_bellIsRung)
        {
            _isInZone = true;
            _ringDoorbellUIController.ShowDoorbellText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_bellIsRung)
        {
            _isInZone = false;
            _timeHeldDown = 0f;
            _ringDoorbellUIController.HideDoorbellText();
            _ringDoorbellUIController.ResetDoorbellText();
        }
    }

    private void Update()
    {
        if (_isInZone && Input.GetKey(KeyCode.R) && !_bellIsRung)
        {
            _timeHeldDown += Time.deltaTime;
            var lerpValue = Mathf.Clamp01(_timeHeldDown / TimeToActivate);
            _ringDoorbellUIController.UpdateDoorbellText(lerpValue);
            
            if (_timeHeldDown >= TimeToActivate)
            {
                RingDoorBell();
            }
        }

        if (Input.GetKeyUp(KeyCode.R) && !_bellIsRung)
        {
            _timeHeldDown = 0f;
            _ringDoorbellUIController.ResetDoorbellText();
        }
    }

    private void RingDoorBell()
    {
        _bellIsRung = true;
        _particles.Stop();
        _ringDoorbellUIController.HideDoorbellText();
        AudioManager.Instance.PlayDoorbellAudio();
        _fenceAnim.SetTrigger("raiseFence");
        _houseSpawner?.StartSpawning();
    }
}
