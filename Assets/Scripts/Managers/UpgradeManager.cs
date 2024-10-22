using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    
    [SerializeField] private TMP_Text _xpText;
    [SerializeField] private TMP_Text _healthLevelText;
    [SerializeField] private TMP_Text _ammoLevelText;
    [SerializeField] private TMP_Text _damageLevelText;
    [SerializeField] private TMP_Text _moveSpeedLevelText;
    [SerializeField] private TMP_Text _shotSpeedLevelText;

    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _player;

    private int _healthPoints;
    private int _ammoPoints;
    private int _damagePoints;
    private int _moveSpeedPoints;
    private int _shotSpeedPoints;

    public int HealthLevel => _healthPoints / PointsPerLevel;
    public int AmmoLevel => _ammoPoints / PointsPerLevel;
    public int DamageLevel => _damagePoints / PointsPerLevel;
    public int MoveSpeedLevel => _moveSpeedPoints / PointsPerLevel;
    public int ShotSpeedLevel => _shotSpeedPoints / PointsPerLevel;
    
    public int PointsAllocated { get; set; }

    private const int PointsPerLevel = 10;

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

    public void EnterShop()
    {
        var playerController = _player.GetComponent<PlayerController>();
        playerController.enabled = false;

        _upgradePanel.SetActive(true);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
    }

    public void ExitShop()
    {
        var playerController = _player.GetComponent<PlayerController>();
        playerController.enabled = true;

        _upgradePanel.SetActive(false);
    }

    private void ApplyUpgrades()
    {
        PlayerStatManager.Instance.ApplyStatUpgrades(HealthLevel, AmmoLevel, DamageLevel, MoveSpeedLevel,
            ShotSpeedLevel);
    }

    private bool IsAbleToSpend()
    {
        return XpManager.Instance.CurrentXp > 0;
    }

    public void UpgradeHealth()
    {
        if (!IsAbleToSpend()) return;

        var wasAtFullHealth = PlayerStatManager.Instance.CurrentHealth >= PlayerStatManager.Instance.CurrentMaxHealth;
        
        _healthLevelText.text = $"{++_healthPoints}";
        PointsAllocated++;
        XpManager.Instance.DecreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();

        if (wasAtFullHealth)
        {
            PlayerStatManager.Instance.RefillHealth();
        }
    }

    public void DowngradeHealth()
    {
        if (_healthPoints <= 0) return;
        _healthLevelText.text = $"{--_healthPoints}";
        PointsAllocated--;
        XpManager.Instance.IncreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void UpgradeAmmo()
    {
        if (!IsAbleToSpend()) return;
        _ammoLevelText.text = $"{++_ammoPoints}";
        PointsAllocated++;
        XpManager.Instance.DecreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void DowngradeAmmo()
    {
        if (_ammoPoints <= 0) return;
        _ammoLevelText.text = $"{--_ammoPoints}";
        PointsAllocated--;
        XpManager.Instance.IncreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void UpgradeDamage()
    {
        if (!IsAbleToSpend()) return;
        _damageLevelText.text = $"{++_damagePoints}";
        PointsAllocated++;
        XpManager.Instance.DecreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void DowngradeDamage()
    {
        if (_damagePoints <= 0) return;
        _damageLevelText.text = $"{--_damagePoints}";
        PointsAllocated--;
        XpManager.Instance.IncreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void UpgradeMoveSpeed()
    {
        if (!IsAbleToSpend()) return;
        _moveSpeedLevelText.text = $"{++_moveSpeedPoints}";
        PointsAllocated++;
        XpManager.Instance.DecreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void DowngradeMoveSpeed()
    {
        if (_moveSpeedPoints <= 0) return;
        _moveSpeedLevelText.text = $"{--_moveSpeedPoints}";
        PointsAllocated--;
        XpManager.Instance.IncreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void UpgradeShotSpeed()
    {
        if (!IsAbleToSpend()) return;
        _shotSpeedLevelText.text = $"{++_shotSpeedPoints}";
        PointsAllocated++;
        XpManager.Instance.DecreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void DowngradeShotSpeed()
    {
        if (_shotSpeedPoints <= 0) return;
        _shotSpeedLevelText.text = $"{--_shotSpeedPoints}";
        PointsAllocated--;
        XpManager.Instance.IncreaseXp(1);
        _xpText.text = $"XP: {XpManager.Instance.CurrentXp}";
        
        ApplyUpgrades();
    }

    public void OnClickConfirmButton()
    {
        ExitShop();
    }
}
