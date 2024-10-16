using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManagerSingleton : MonoBehaviour
{
    public static UpgradeManagerSingleton Instance { get; private set; }

    private const float HealthMultiplier = 1.1f;
    private const float DamageMultiplier = 1.15f;
    private const float MoveSpeedMultiplier = 1.05f;
    private const float ShotSpeedMultiplier = 1.05f;
    private const float AmmoUpgradeCount = 2f;

    [SerializeField] private GameObject _upgradePanel;
    
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
        LevelManagerSingleton.Instance.OnHouseVisited += ActivateUpgradePanel;
    }

    private void OnDisable()
    { 
        LevelManagerSingleton.Instance.OnHouseVisited -= ActivateUpgradePanel;
    }

    public void ActivateUpgradePanel()
    {
        _upgradePanel.SetActive(true);
    }

    public void UpgradeHealth()
    {
        PlayerStatManagerSingleton.Instance.CurrentMaxHealth = Mathf.Round(PlayerStatManagerSingleton.Instance.CurrentMaxHealth * HealthMultiplier);
    }
    
    public void UpgradeMoveSpeed()
    {
        PlayerStatManagerSingleton.Instance.CurrentMoveSpeed = Mathf.Round(PlayerStatManagerSingleton.Instance.CurrentMoveSpeed * MoveSpeedMultiplier);
    }

    public void UpgradeDamage()
    {
        PlayerStatManagerSingleton.Instance.CurrentDamage = Mathf.Round(PlayerStatManagerSingleton.Instance.CurrentDamage * DamageMultiplier);
    }

    public void UpgradeAmmo()
    {
        PlayerStatManagerSingleton.Instance.CurrentMaxAmmo += AmmoUpgradeCount;
    }
    
    public void UpgradeShotSpeed()
    {
        PlayerStatManagerSingleton.Instance.CurrentShotSpeed = Mathf.Round(PlayerStatManagerSingleton.Instance.CurrentShotSpeed * ShotSpeedMultiplier);
    }
}
