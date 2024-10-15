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
        PlayerStatManagerSingleton.Instance.OnHealthChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        if (PlayerStatManagerSingleton.Instance is not null)
        {
            PlayerStatManagerSingleton.Instance.OnHealthChanged -= UpdateHealthText;
        }
    }

    private void Start()
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"Health: {PlayerStatManagerSingleton.Instance.CurrentHealth} / {PlayerStatManagerSingleton.Instance.CurrentMaxHealth}";
    }
}
