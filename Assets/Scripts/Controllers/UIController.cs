using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _killedText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerShootingController _playerShootingController;

    private void OnEnable()
    {
        StatManager.Instance.OnPlayerDamaged += UpdateHealthText;
        StatManager.Instance.OnEnemyKilled += UpdateKilledText;
        _playerShootingController.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        StatManager.Instance.OnPlayerDamaged -= UpdateHealthText;
        StatManager.Instance.OnEnemyKilled -= UpdateKilledText;
        _playerShootingController.OnAmmoChanged -= UpdateAmmoText;
    }

    private void Start()
    {
        InitializeTexts();
    }

    private void InitializeTexts()
    {
        _healthText.text = "Health: 100";
        _killedText.text = "Enemies Killed: 0";
        _ammoText.text = $"Ammo: {StatManager.Instance.MaxAmmo}";
    }

    private void UpdateHealthText(int playerHealth)
    {
        _healthText.text = $"Health: {playerHealth}";
    }

    private void UpdateKilledText(int numKilled)
    {
        _killedText.text = $"Enemies Killed: {numKilled}";
    }

    private void UpdateAmmoText(int currentAmmo)
    {
        _ammoText.text = $"Ammo: {currentAmmo}";
    }
}
