using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager Instance { get; private set; }
    
    public int CurrentXp { get; set; }
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

    public void DecreaseXp(int amount)
    {
        CurrentXp = Mathf.Max(0, CurrentXp - amount);
        OnXpChanged?.Invoke();
    }
}
