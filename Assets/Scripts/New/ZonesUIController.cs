using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZonesUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _zoneText;

    private void OnEnable()
    {
        LevelManager.Instance.OnHouseVisited += UpdateZonesText;
    }
    
    private void OnDisable()
    {
        if (LevelManager.Instance is not null)
        {
            LevelManager.Instance.OnHouseVisited -= UpdateZonesText;
        }
    }

    private void Start()
    {
        UpdateZonesText();
    }

    private void UpdateZonesText()
    {
        _zoneText.text = $"Zones Remaining: {LevelManager.Instance.HousesRemaining}";
    }
}
