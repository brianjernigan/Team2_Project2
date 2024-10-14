using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private const float HealthMultiplier = 1.1f;
    private const float DamageMultiplier = 1.15f;
    private const float MoveSpeedMultiplier = 1.05f;
    private const float ShotSpeedMultiplier = 1.05f;
    private const float AmmoUpgradeCount = 2f;

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

    public void UpgradeHealth()
    {
        PlayerStats.Instance.CurrentMaxHealth = Mathf.Round(PlayerStats.Instance.CurrentMaxHealth * HealthMultiplier);
    }
    
    public void UpgradeMoveSpeed()
    {
        PlayerStats.Instance.CurrentMoveSpeed = Mathf.Round(PlayerStats.Instance.CurrentMoveSpeed * MoveSpeedMultiplier);
    }

    public void UpgradeDamage()
    {
        PlayerStats.Instance.CurrentDamage = Mathf.Round(PlayerStats.Instance.CurrentDamage * DamageMultiplier);
    }

    public void UpgradeAmmo()
    {
        PlayerStats.Instance.CurrentMaxAmmo += AmmoUpgradeCount;
    }
    
    public void UpgradeShotSpeed()
    {
        PlayerStats.Instance.CurrentShotSpeed = Mathf.Round(PlayerStats.Instance.CurrentShotSpeed * ShotSpeedMultiplier);
    }
}
