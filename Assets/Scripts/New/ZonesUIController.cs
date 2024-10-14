using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZonesUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _zoneText;

    // private void OnEnable()
    // {
    //     StatManager.Instance.OnZoneActivated += UpdateZonesText;
    // }
    //
    // private void OnDisable()
    // {
    //     if (StatManager.Instance is not null)
    //     {
    //         StatManager.Instance.OnZoneActivated += UpdateZonesText;
    //     }
    // }

    private void Start()
    {
        UpdateZonesText();
    }

    private void UpdateZonesText()
    {
        _zoneText.text = $"Zones Remaining: {LevelManager.Instance.HousesRemaining}";
    }
}
