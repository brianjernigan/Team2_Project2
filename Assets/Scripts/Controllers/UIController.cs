using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private GameObject _player;

    private void OnEnable()
    {
        StatManager.Instance.OnPlayerDamaged += UpdateHealthText;
    }

    private void OnDisable()
    {
        StatManager.Instance.OnPlayerDamaged -= UpdateHealthText;
    }

    private void Start()
    {
        _healthText.text = "Health: 100";
    }

    private void UpdateHealthText(int playerHealth)
    {
        _healthText.text = $"Health: {playerHealth}";
    }
}
