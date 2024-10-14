using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private const float BaseMaxHealth = 25f;
    private const float BaseMaxAmmo = 10f;
    private const float BaseMoveSpeed = 10f;
    private const float BaseDamage = 10f;
    private const float BaseShotSpeed = 10f;
    
    public float CurrentMaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float CurrentMaxAmmo { get; set; }
    public float CurrentAmmo { get; set; }
    public float CurrentMoveSpeed { get; set; }
    public float CurrentDamage { get; set; }
    public float CurrentShotSpeed { get; set; }

    public event Action OnPlayerDamaged;

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

    private void Start()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        CurrentMaxHealth = BaseMaxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentMaxAmmo = BaseMaxAmmo;
        CurrentAmmo = CurrentMaxAmmo;
        CurrentMoveSpeed = BaseMoveSpeed;
        CurrentDamage = BaseDamage;
        CurrentShotSpeed = BaseShotSpeed;
    }

    public void ResetStats()
    {
        CurrentHealth = CurrentMaxHealth;
        CurrentAmmo = CurrentMaxAmmo;
    }

    public void DamagePlayer(float amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Round(amount));

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.OnPlayerDeath();
        }
        else
        {
            OnPlayerDamaged?.Invoke();
        }
    }

    public void HealPlayer(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentMaxHealth, CurrentHealth + amount);
    }
}
