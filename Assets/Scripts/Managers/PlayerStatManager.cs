using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance { get; private set; }

    [SerializeField] private GameObject _fadePanel;

    private const float FadeDuration = 2.25f;
    
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
    public bool IsDead => CurrentHealth <= 0;
    
    private const float ReloadDuration = 1.75f;

    private const float HealthMultiplier = 0.3f;
    private const float DamageMultiplier = 0.25f;
    private const int AmmoPerLevel = 2;
    private const float MoveSpeedMultiplier = 0.1f;
    private const float ShotSpeedMultiplier = 0.1f;
    
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
        CurrentAmmo = Mathf.Max(0, CurrentAmmo - 1);
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
        RefillHealth();
        RefillAmmo();
    }

    public void ApplyStatUpgrades(int healthLevel, int ammoLevel, int damageLevel, int moveSpeedLevel,
        int shotSpeedLevel)
    {
        CurrentMaxHealth = BaseMaxHealth * (1 + healthLevel * HealthMultiplier);
        CurrentDamage = BaseDamage * (1 + damageLevel * DamageMultiplier);
        CurrentMoveSpeed = BaseMoveSpeed * (1 + moveSpeedLevel * MoveSpeedMultiplier);
        CurrentShotSpeed = BaseShotSpeed * (1 + shotSpeedLevel * ShotSpeedMultiplier);
        CurrentMaxAmmo = BaseMaxAmmo + (ammoLevel * AmmoPerLevel);

        CurrentHealth = Mathf.Min(CurrentHealth, CurrentMaxHealth);
        CurrentAmmo = Mathf.Min(CurrentAmmo, CurrentMaxAmmo);
        
        OnHealthChanged?.Invoke();
        OnAmmoChanged?.Invoke();
    }

    public void DamagePlayer(float amount, GameObject enemy)
    {
        if (_isInvulnerable) return;
        if (IsDead) return;
        
        AudioManager.Instance.PlayPlayerHitAudio();
        
        // Calls on enemy only, not bullet
        if (enemy.TryGetComponent<EnemyController>(out var enemyController))
        {
            enemyController.TriggerAttackAnimation();
        }
        
        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Round(amount));
        OnHealthChanged?.Invoke();
        
        if (CurrentHealth <= 0)
        {
            _playerAnimator.SetTrigger("die");
            StartCoroutine(PlayerDeathRoutine());
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        AudioManager.Instance.PlayDeathAudio();
        DestroyAllEnemies();
        _playerAnimator.SetTrigger("die");
        
        yield return StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        _fadePanel.SetActive(true);
        
        var elapsedTime = 0f;
        var originalColor = _fadePanel.GetComponent<Image>().color;

        while (elapsedTime < FadeDuration)
        {
            var newAlpha = Mathf.Lerp(0, 1, elapsedTime / FadeDuration);
            _fadePanel.GetComponent<Image>().color =
                new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.OnPlayerDeath();

        yield return new WaitForSeconds(0.1f);
        
        _fadePanel.SetActive(false);
    }

    private void DestroyAllEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
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

    public void RefillHealth()
    {
        CurrentHealth = CurrentMaxHealth;
        OnHealthChanged?.Invoke();
    }
    
    private void RefillAmmo()
    {
        CurrentAmmo = CurrentMaxAmmo;
        OnAmmoChanged?.Invoke();
    }
}
