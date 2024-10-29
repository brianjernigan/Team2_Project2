using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;

    private void OnEnable()
    {
        PlayerStatManager.Instance.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (PlayerStatManager.Instance is not null)
        {
            PlayerStatManager.Instance.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        _healthBarFill.fillAmount =
            Mathf.Clamp01(PlayerStatManager.Instance.CurrentHealth / PlayerStatManager.Instance.CurrentMaxHealth);
    }
}
