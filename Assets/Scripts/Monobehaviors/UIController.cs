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
    [SerializeField] private TMP_Text _xpRequiredText;
    
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
        UpdateAllTexts();
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"Health: {StatManager.Instance.CurrentHealth} / {StatManager.Instance.CurrentMaxHealth}";
    }

    private void UpdateKilledText()
    {
        _killedText.text = $"Enemies Killed: {StatManager.Instance.NumEnemiesKilled}";
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = $"Ammo: {StatManager.Instance.CurrentAmmo} / {StatManager.Instance.CurrentMaxAmmo}";
    }

    private void UpdateXpText()
    {
        _xpText.text = $"XP: {StatManager.Instance.CurrentXp}";
    }

    private void UpdateXpRequiredText()
    {
        _xpRequiredText.text = $"XP Required: {StatManager.Instance.XpThreshold}";
    }
    
    private void ActivateUpgradePanel()
    {
        _gamePanel.SetActive(false);
        _upgradePanel.SetActive(true);
        _playerShootingController.enabled = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _gamePanel.SetActive(true);
        _upgradePanel.SetActive(false);
        _playerShootingController.enabled = true;
        UpdateAllTexts();
        Time.timeScale = 1;
    }

    private void UpdateAllTexts()
    {
        UpdateAmmoText();
        UpdateHealthText();
        UpdateKilledText();
        UpdateXpText();
        UpdateXpRequiredText();
    }
}
