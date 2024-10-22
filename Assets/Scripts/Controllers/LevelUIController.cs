using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;

    private void OnEnable()
    {
        XpManager.Instance.OnPlayerLevelChanged += UpdateLevelText;
    }

    private void OnDisable()
    {
        if (XpManager.Instance is not null)
        {
            XpManager.Instance.OnPlayerLevelChanged -= UpdateLevelText;
        }
    }

    private void Start()
    {
        UpdateLevelText();
    }
    
    private void UpdateLevelText()
    {
        _levelText.text = $"Current Level: {XpManager.Instance.CurrentPlayerLevel}";
    }
}
