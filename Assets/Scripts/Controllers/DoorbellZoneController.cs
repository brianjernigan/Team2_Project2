using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorbellZoneController : MonoBehaviour
{
    private const float TimeToActivate = 1f;

    private float _timeHeldDown;
    private bool _isInZone;
    private bool _bellIsRung;

    [SerializeField] private Animator _fenceAnim;
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private HouseSpawner _houseSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_bellIsRung)
        {
            _isInZone = true;
            _interactionPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_bellIsRung)
        {
            _isInZone = false;
            _interactionPanel.SetActive(false);
            _timeHeldDown = 0f;
        }
    }

    private void Update()
    {
        if (_isInZone && Input.GetKey(KeyCode.R) && !_bellIsRung)
        {
            _timeHeldDown += Time.deltaTime;

            if (_timeHeldDown >= TimeToActivate)
            {
                RingDoorBell();
            }
        }
    }

    private void RingDoorBell()
    {
        _bellIsRung = true;
        AudioManagerSingleton.Instance.PlayDoorbellAudio();
        _fenceAnim.SetTrigger("raiseFence");
        _interactionPanel.SetActive(false);

        _houseSpawner?.StartSpawning();
    }
}
