using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    #region UpgradeableStats

    public float BaseHealth { get; set; } = 50f;
    public float CurrentHealth { get; set; }

    public float BaseDamage { get; set; } = 10f;
    public float CurrentDamage { get; set; }

    public float BaseAmmo { get; set; } = 10f;
    public float CurrentAmmo { get; set; }

    public float BaseShotSpeed { get; set; } = 10f;
    public float CurrentShotSpeed { get; set; }

    #endregion
    
    private const float DamageCooldown = 0.5f;
    private bool _canTakeDamage = true;
    private Coroutine _invulnerabilityCoroutine;
    
    public event Action<float> OnPlayerDamaged;

    private int _numEnemiesKilled;
    public event Action<int> OnEnemyKilled;

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

    private void Start()
    {
        CurrentHealth = BaseHealth;
        CurrentDamage = BaseDamage;
        CurrentAmmo = BaseAmmo;
        CurrentShotSpeed = BaseShotSpeed;

        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    
    public void DamagePlayer(float amount)
    {
        if (!_canTakeDamage) return;
        
        CurrentHealth -= amount;
        OnPlayerDamaged?.Invoke(CurrentHealth);
        
        _audio.PlayPlayerHitAudio();
        
        Debug.Log($"Player takes {amount} damage");

        _invulnerabilityCoroutine ??= StartCoroutine(PlayerInvulnerability());
            
        if (CurrentHealth <= 0)
        {
            Die();
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

    private void Die()
    {
        Debug.Log("Player is dead");
    }
}
