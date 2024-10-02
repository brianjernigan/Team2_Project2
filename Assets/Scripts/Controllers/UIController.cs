using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _killedText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _xpText;
    
    [Header("Player Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerShootingController _playerShootingController;

    [Header("Panels")] 
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _upgradePanel;
    
    private void OnEnable()
    {
        StatManager.Instance.OnPlayerDamaged += UpdateHealthText;
        StatManager.Instance.OnEnemyKilled += UpdateKilledText;
        StatManager.Instance.OnXpChanged += UpdateXpText;
        StatManager.Instance.OnPlayerUpgrade += ActivateUpgradePanel;
        _playerShootingController.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        StatManager.Instance.OnPlayerDamaged -= UpdateHealthText;
        StatManager.Instance.OnEnemyKilled -= UpdateKilledText;
        StatManager.Instance.OnXpChanged -= UpdateXpText;
        StatManager.Instance.OnPlayerUpgrade -= ActivateUpgradePanel;
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
        _ammoText.text = $"Ammo: {StatManager.Instance.CurrentAmmo}";
        _xpText.text = "XP: 0";
    }

    private void UpdateHealthText(float playerHealth)
    {
        _healthText.text = $"Health: {playerHealth}";
    }

    private void UpdateKilledText(int numKilled)
    {
        _killedText.text = $"Enemies Killed: {numKilled}";
    }

    private void UpdateAmmoText(float currentAmmo)
    {
        _ammoText.text = $"Ammo: {currentAmmo}";
    }

    private void UpdateXpText()
    {
        _xpText.text = $"XP: {StatManager.Instance.CurrentXp}";
    }
    
    private void ActivateUpgradePanel()
    {
        _gamePanel.SetActive(false);
        _upgradePanel.SetActive(true);
    }
}
