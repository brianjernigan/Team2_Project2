using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    
    [Header("Texts")] [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _killedText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _xpText;

    [Header("Player Components")] 
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerShootingController _playerShootingController;
    [SerializeField] private ChainController _chainController;

    [Header("Panels")] [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _upgradePanel;

    [Header("Images")] 
    [SerializeField] private Image _chainImage;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        StatManager.Instance.OnPlayerDamaged += UpdateHealthText;
        StatManager.Instance.OnEnemyKilled += UpdateKilledText;
        StatManager.Instance.OnXpChanged += UpdateXpText;
        StatManager.Instance.OnPlayerUpgrade += ActivateUpgradePanel;
        _playerShootingController.OnAmmoChanged += UpdateAmmoText;
        _chainController.OnChainTriggered += HandleChainTriggered;
    }

    private void OnDisable()
    {
        StatManager.Instance.OnPlayerDamaged -= UpdateHealthText;
        StatManager.Instance.OnEnemyKilled -= UpdateKilledText;
        StatManager.Instance.OnXpChanged -= UpdateXpText;
        StatManager.Instance.OnPlayerUpgrade -= ActivateUpgradePanel;
        _playerShootingController.OnAmmoChanged -= UpdateAmmoText;
        _chainController.OnChainTriggered -= HandleChainTriggered;
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
        _xpText.text = $"XP: {StatManager.Instance.CurrentXp} / {StatManager.Instance.XpThreshold}";
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

    public void UpdateAllTexts()
    {
        UpdateAmmoText();
        UpdateHealthText();
        UpdateKilledText();
        UpdateXpText();
    }
    
    private void HandleChainTriggered()
    {
        StartCoroutine(UpdateChainImageAlpha(_chainController.ChainCooldown));
    }

    private IEnumerator UpdateChainImageAlpha(float duration)
    {
        var color = _chainImage.color;
        color = Color.red;
        color.a = 0f;
        _chainImage.color = color;

        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            _chainImage.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        _chainImage.color = color;

        _chainImage.color = Color.white;
    }
}
