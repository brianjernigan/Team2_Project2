using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }
    
    private const int MaxHealth = 100;
    public int PlayerHealth { get; set; }
    
    private const int BaseDamage = 10;
    public int CurrentDamage { get; set; }

    private const float DamageCooldown = 2.0f;
    private bool _canTakeDamage = true;
    private Coroutine _invulnerabilityCoroutine;
    
    public event Action<int> OnPlayerDamaged;

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
        PlayerHealth = MaxHealth;
        CurrentDamage = BaseDamage;

        _audio = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    
    public void DamagePlayer(int amount)
    {
        if (!_canTakeDamage) return;
        
        PlayerHealth -= amount;
        OnPlayerDamaged?.Invoke(PlayerHealth);
        
        _audio.PlayPlayerHitAudio();
        
        Debug.Log($"Player takes {amount} damage");

        _invulnerabilityCoroutine ??= StartCoroutine(PlayerInvulnerability());
            
        if (PlayerHealth <= 0)
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
