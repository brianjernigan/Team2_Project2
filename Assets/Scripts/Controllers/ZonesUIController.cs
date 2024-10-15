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
        LevelManagerSingleton.Instance.OnHouseVisited += UpdateZonesText;
    }
    
    private void OnDisable()
    {
        if (LevelManagerSingleton.Instance is not null)
        {
            LevelManagerSingleton.Instance.OnHouseVisited -= UpdateZonesText;
        }
    }

    private void Start()
    {
        UpdateZonesText();
    }

    private void UpdateZonesText()
    {
        _zoneText.text = $"Zones Remaining: {LevelManagerSingleton.Instance.HousesRemaining}";
    }
}
