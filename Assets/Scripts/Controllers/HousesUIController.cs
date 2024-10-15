using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HousesUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _housesText;

    private void OnEnable()
    {
        LevelManagerSingleton.Instance.OnHouseVisited += UpdateHousesText;
    }
    
    private void OnDisable()
    {
        if (LevelManagerSingleton.Instance is not null)
        {
            LevelManagerSingleton.Instance.OnHouseVisited -= UpdateHousesText;
        }
    }

    private void Start()
    {
        UpdateHousesText();
    }

    private void UpdateHousesText()
    {
        _housesText.text = $"Houses Remaining: {LevelManagerSingleton.Instance.HousesRemaining}";
    }
}
