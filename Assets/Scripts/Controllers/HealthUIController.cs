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
        PlayerStatManager.Instance.OnHealthChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        if (PlayerStatManager.Instance is not null)
        {
            PlayerStatManager.Instance.OnHealthChanged -= UpdateHealthText;
        }
    }

    private void Start()
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"Health: {PlayerStatManager.Instance.CurrentHealth} / {PlayerStatManager.Instance.CurrentMaxHealth}";
    }
}
