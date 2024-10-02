using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    public event Action<int> OnEnemyKilled;
    public event Action OnXpChanged;
    public event Action<float> OnPlayerDamaged;
    public event Action OnPlayerUpgrade;
    
    private const float BaseMoveSpeed = 10f;
    private const float BaseHealth = 50f;
    private const float BaseDamage = 10f;
    private const float BaseAmmo = 10f;
    private const float BaseShotSpeed = 10f;
    
    #region UpgradeableStats
    public float CurrentMoveSpeed { get; set; }
    public float CurrentHealth { get; set; }
    public float CurrentDamage { get; set; }
    public float CurrentAmmo { get; set; }
    public float CurrentShotSpeed { get; set; }
    #endregion
    
    public int CurrentXp { get; set; }
    
    private const float DamageCooldown = 1.0f;
    private bool _canTakeDamage = true;
    private Coroutine _invulnerabilityCoroutine;

    private int _numEnemiesKilled;
    
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
        CurrentHealth = BaseHealth;
        CurrentDamage = BaseDamage;
        CurrentAmmo = BaseAmmo;
        CurrentShotSpeed = BaseShotSpeed;
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
        OnPlayerDamaged?.Invoke(CurrentHealth);
        
        _audio.PlayPlayerHitAudio();

        _invulnerabilityCoroutine ??= StartCoroutine(PlayerInvulnerability());
            
        if (CurrentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    public void EnemyDied()
    {
        OnEnemyKilled?.Invoke(++_numEnemiesKilled);
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
        Debug.Log("Player is dead");
    }

    public void IncreaseXp(int amount)
    {
        CurrentXp += amount;
        _audio.PlayCollectAudio();
        OnXpChanged?.Invoke();

        // if (CurrentXp >= 10)
        // {
        //     OnPlayerUpgrade?.Invoke();
        // }
    }
}
