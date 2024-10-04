using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    public event Action OnEnemyKilled;
    public event Action OnXpChanged;
    public event Action OnPlayerDamaged;
    public event Action OnPlayerUpgrade;

    private const float BaseMaxHealth = 25f;
    private const float BaseMaxAmmo = 10f;
    private const float BaseMoveSpeed = 10f;
    private const float BaseDamage = 10f;
    private const float BaseShotSpeed = 10f;
    
    #region UpgradeableStats
    public float CurrentMoveSpeed { get; set; }
    public float MaxHealth { get; set; } = BaseMaxHealth;
    public float CurrentHealth { get; set; }
    public float CurrentDamage { get; set; }
    public float MaxAmmo { get; set; } = BaseMaxAmmo;
    public float CurrentAmmo { get; set; }
    public float CurrentShotSpeed { get; set; }
    #endregion
    
    private const int BaseXpThreshold = 10;
    public int CurrentPlayerLevel { get; set; }
    public int CurrentXp { get; set; }
    public int XpThreshold { get; set; } = BaseXpThreshold;
    
    private const float XpMultiplier = 1.75f;
    private const float HealthMultiplier = 1.1f;
    private const float DamageMultiplier = 1.05f;
    private const float ShotSpeedMultiplier = 1.05f;
    private const float MoveSpeedMultiplier = 1.05f;
    private const float AmmoUpgradeCount = 2;
    
    private const float DamageCooldown = 1.0f;
    private bool _canTakeDamage = true;
    private Coroutine _invulnerabilityCoroutine;

    public int NumEnemiesKilled { get; set; }

    private AudioManager _audio;
    
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

    private void InitializeStats()
    {
        CurrentMoveSpeed = BaseMoveSpeed;
        CurrentHealth = BaseMaxHealth;
        CurrentDamage = BaseDamage;
        CurrentAmmo = BaseMaxAmmo;
        CurrentShotSpeed = BaseShotSpeed;
        CurrentPlayerLevel = 1;
    }

    private void Start()
    {
        InitializeStats();
        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void DamagePlayer(float amount)
    {
        if (!_canTakeDamage) return;
        
        CurrentHealth -= amount;
        OnPlayerDamaged?.Invoke();
        
        _audio.PlayPlayerHitAudio();

        _invulnerabilityCoroutine ??= StartCoroutine(PlayerInvulnerability());
            
        if (CurrentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    public void EnemyDied()
    {
        NumEnemiesKilled++;
        OnEnemyKilled?.Invoke();
    }

    private IEnumerator PlayerInvulnerability()
    {
        _canTakeDamage = false;

        yield return new WaitForSeconds(DamageCooldown);

        _canTakeDamage = true;
        _invulnerabilityCoroutine = null;
    }

    private void OnPlayerDeath()
    {
        InitializeStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseXp(int amount)
    {
        CurrentXp += amount;
        _audio.PlayCollectAudio();
        OnXpChanged?.Invoke();

        if (CurrentXp < XpThreshold) return;
        IncreaseXpThreshold(++CurrentPlayerLevel);
        OnPlayerUpgrade?.Invoke();
    }

    private void IncreaseXpThreshold(int level)
    {
        XpThreshold = Mathf.RoundToInt(BaseXpThreshold * Mathf.Pow(XpMultiplier, level - 1));
    }

    public void UpgradeHealth()
    {
        MaxHealth = Mathf.Round(MaxHealth * HealthMultiplier);
    }

    public void UpgradeMoveSpeed()
    {
        CurrentMoveSpeed = Mathf.Round(CurrentMoveSpeed * MoveSpeedMultiplier);
    }

    public void UpgradeDamage()
    {
        CurrentDamage = Mathf.Round(CurrentDamage * DamageMultiplier);
    }

    public void UpgradeAmmo()
    {
        MaxAmmo += AmmoUpgradeCount;
    }

    public void UpgradeShotSpeed()
    {
        CurrentShotSpeed = Mathf.Round(CurrentShotSpeed * ShotSpeedMultiplier);
    }
}
