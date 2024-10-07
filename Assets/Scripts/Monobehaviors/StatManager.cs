using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int MoveSpeedLevel { get; private set; }
    public float CurrentMaxHealth { get; set; }
    public int MaxHealthLevel { get; private set; }
    public float CurrentHealth { get; set; }
    public float CurrentDamage { get; set; }
    public int DamageLevel { get; private set; }
    public float CurrentMaxAmmo { get; set; }
    public int AmmoLevel { get; private set; }
    public float CurrentAmmo { get; set; }
    public float CurrentShotSpeed { get; set; }
    public int ShotSpeedLevel { get; private set; }
    public float CurrentPickupDuration { get; set; } = 10f;
    #endregion
    
    private const int BaseXpThreshold = 10;
    public int CurrentPlayerLevel { get; set; }
    public int CurrentXp { get; set; }
    public int XpThreshold { get; set; } = BaseXpThreshold;
    
    private const float XpMultiplier = 1.55f;
    private const float HealthMultiplier = 1.1f;
    private const float DamageMultiplier = 1.15f;
    private const float ShotSpeedMultiplier = 1.05f;
    private const float MoveSpeedMultiplier = 1.05f;
    private const float AmmoUpgradeCount = 2;
    
    private const float DamageCooldown = 1.0f;
    private bool _canTakeDamage = true;
    private Coroutine _invulnerabilityCoroutine;

    public int NumEnemiesKilled { get; set; }
    
    public bool GameIsOver { get; set; }

    private AudioManager _audio;

    private bool _levelLoading = false;

    [SerializeField] private UpgradeHudController _upgradeHud;
    
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

    private void InitializePlayerStats()
    {
        CurrentMoveSpeed = BaseMoveSpeed;
        CurrentMaxHealth = BaseMaxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentDamage = BaseDamage;
        CurrentMaxAmmo = BaseMaxAmmo;
        CurrentAmmo = CurrentMaxAmmo;
        CurrentShotSpeed = BaseShotSpeed;
        CurrentPlayerLevel = 1;
        CurrentXp = 0;
        XpThreshold = BaseXpThreshold;
        UIController.Instance.UpdateAllTexts();
    }

    private void Start()
    {
        InitializePlayerStats();
        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        _audio.PlayLevelMusic();
    }

    public void DamagePlayer(float amount)
    {
        if (!_canTakeDamage) return;

        amount = Mathf.Round(amount);
        
        CurrentHealth -= amount;
        OnPlayerDamaged?.Invoke();
        
        _audio.PlayPlayerHitAudio();

        _invulnerabilityCoroutine ??= StartCoroutine(PlayerInvulnerability());
            
        if (CurrentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    private void Update()
    {
        // Testing
        // if (NumEnemiesKilled >= 5 && !_levelLoading)
        // {
        //     LoadNextLevel("L2");
        // }
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
        InitializePlayerStats();
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
        CurrentMaxHealth = Mathf.Round(CurrentMaxHealth * HealthMultiplier);
        _upgradeHud.UpdateStatHud(0, ++MaxHealthLevel);
    }

    public void UpgradeMoveSpeed()
    {
        CurrentMoveSpeed = Mathf.Round(CurrentMoveSpeed * MoveSpeedMultiplier);
        _upgradeHud.UpdateStatHud(1, ++MoveSpeedLevel);
    }

    public void UpgradeDamage()
    {
        CurrentDamage = Mathf.Round(CurrentDamage * DamageMultiplier);
        _upgradeHud.UpdateStatHud(2, ++DamageLevel);
    }

    public void UpgradeAmmo()
    {
        CurrentMaxAmmo += AmmoUpgradeCount;
        _upgradeHud.UpdateStatHud(3, ++AmmoLevel);
    }

    public void UpgradeShotSpeed()
    {
        CurrentShotSpeed = Mathf.Round(CurrentShotSpeed * ShotSpeedMultiplier);
        _upgradeHud.UpdateStatHud(4, ++ShotSpeedLevel);
    }

    private void LoadNextLevel(string levelName)
    {
        _levelLoading = true;
        SceneManager.LoadScene(levelName);
    }
}
