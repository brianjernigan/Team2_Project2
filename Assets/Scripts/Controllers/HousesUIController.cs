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
        LevelManager.Instance.OnHouseVisited += UpdateHousesText;
    }
    
    private void OnDisable()
    {
        if (LevelManager.Instance is not null)
        {
            LevelManager.Instance.OnHouseVisited -= UpdateHousesText;
        }
    }

    private void Start()
    {
        UpdateHousesText();
    }

    private void UpdateHousesText()
    {
        _housesText.text = $"{LevelManager.Instance.HousesRemaining}";
    }
}
