using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private const int MaxHealth = 100;
    public int PlayerHealth { get; set; }
    public static StatManager Instance { get; private set; }
    public event Action<int> OnPlayerDamaged;

    private float _damageCooldown = 2.0f;

    private bool _canTakeDamage = true;
    private float _cooldownTimer;

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
        _cooldownTimer = 0f;
    }

    private void Update()
    {
        if (!_canTakeDamage)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _canTakeDamage = true;
            }
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (!_canTakeDamage) return;
        PlayerHealth -= amount;
        OnPlayerDamaged?.Invoke(PlayerHealth);
        Debug.Log("Player takes damage");

        _canTakeDamage = false;
        _cooldownTimer = _damageCooldown;
            
        if (PlayerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead");
    }
}
