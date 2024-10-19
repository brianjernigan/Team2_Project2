using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManagerSingleton : MonoBehaviour
{
    public static ExperienceManagerSingleton Instance { get; private set; }
    
    public int CurrentXp { get; private set; }
    public int CurrentPlayerLevel { get; private set; }

    public event Action OnXpChanged;

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
        InitializeXp();
    }

    public void InitializeXp()
    {
        CurrentPlayerLevel = 1;
        CurrentXp = 0;
        OnXpChanged?.Invoke();
    }

    public void IncreaseXp(int amount)
    {
        CurrentXp += amount;
        OnXpChanged?.Invoke();
    }
}
