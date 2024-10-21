using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance { get; private set; }

    private const float BaseMaxHealth = 10f;
    private const float BaseMaxAmmo = 5f;
    private const float BaseMoveSpeed = 7.5f;
    private const float BaseDamage = 2f;
    private const float BaseShotSpeed = 3f;
    
    public float CurrentMaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float CurrentMaxAmmo { get; set; }
    public float CurrentAmmo { get; set; }
    public float CurrentMoveSpeed { get; set; }
    public float CurrentDamage { get; set; }
    public float CurrentShotSpeed { get; set; }

    private const float InvincibilityDuration = 0.5f;
    private bool _isInvulnerable;

    public bool IsReloading { get; set; }
    private const float ReloadDuration = 1.75f;
    
    public event Action OnAmmoChanged;
    public event Action OnHealthChanged;
    
    private Animator _playerAnimator; 

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
        OnAmmoChanged?.Invoke();
        OnHealthChanged?.Invoke();
        _playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsReloading)
        {
            Reload();
        }
    }

    public bool HasAmmo()
    {
        return CurrentAmmo > 0;
    }

    public void DecreaseAmmo()
    {
        CurrentAmmo--;
        CurrentAmmo = Mathf.Max(0, CurrentAmmo);
        OnAmmoChanged?.Invoke();
    }
    
    private void Reload()
    {
        if (CurrentAmmo >= CurrentMaxAmmo) return;

        StartCoroutine(ReloadRoutine());
    }
    
    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        AudioManager.Instance.PlayReloadAudio();
        
        yield return new WaitForSeconds(ReloadDuration);

        IsReloading = false;
        CurrentAmmo = CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }

    private void InitializeStats()
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
        _playerAnimator.SetBool("isDead", false);
    }

    public void ApplyStatUpgrades(int healthLevel, int ammoLevel, int damageLevel, int moveSpeedLevel,
        int shotSpeedLevel)
    {
        const float levelMultiplier = 0.1f;
        const int ammoPerLevel = 2;
        
        CurrentMaxHealth = BaseMaxHealth * (1 + healthLevel * levelMultiplier);
        CurrentDamage = BaseDamage * (1 + damageLevel * levelMultiplier);
        CurrentMoveSpeed = BaseMoveSpeed * (1 + moveSpeedLevel * levelMultiplier);
        CurrentShotSpeed = BaseShotSpeed * (1 + shotSpeedLevel * levelMultiplier);
        CurrentMaxAmmo = BaseMaxAmmo + (ammoLevel * ammoPerLevel);

        CurrentHealth = Mathf.Min(CurrentHealth, CurrentMaxHealth);
        CurrentAmmo = Mathf.Min(CurrentAmmo, CurrentMaxAmmo);
        
        OnHealthChanged?.Invoke();
        OnAmmoChanged?.Invoke();
    }

    public void DamagePlayer(float amount)
    {
        if (_isInvulnerable) return;
        
        AudioManager.Instance.PlayPlayerHitAudio();
        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Round(amount));

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.OnPlayerDeath();
            _playerAnimator.SetBool("isDead", true); //new
        }
        else
        {
            OnHealthChanged?.Invoke();
            StartCoroutine(InvincibilityRoutine());
        }
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
