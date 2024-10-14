using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDamaged += UpdateHealthText;
    }

    private void OnDisable()
    {
        if (PlayerStats.Instance is not null)
        {
            PlayerStats.Instance.OnPlayerDamaged -= UpdateHealthText;
        }
    }

    private void Start()
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"Health: {PlayerStats.Instance.CurrentHealth} / {PlayerStats.Instance.CurrentMaxHealth}";
    }
}
