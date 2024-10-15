using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManagerSingleton : MonoBehaviour
{
    public static PlayerStatManagerSingleton Instance { get; private set; }

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

    private const float InvincibilityDuration = 0.5f;
    private bool _isInvulnerable;

    public event Action OnHealthChanged;

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
        if (_isInvulnerable) return;
        
        AudioManagerSingleton.Instance.PlayPlayerHitAudio();
        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Round(amount));

        if (CurrentHealth <= 0)
        {
            GameManagerSingleton.Instance.OnPlayerDeath();
        }
        else
        {
            OnHealthChanged?.Invoke();
        }

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        _isInvulnerable = false;
    }

    public void HealPlayer(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentMaxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke();
    }
}
