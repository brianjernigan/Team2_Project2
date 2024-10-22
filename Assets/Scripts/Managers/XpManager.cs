using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager Instance { get; private set; }

    private const int XpPerLevel = 10;
    
    public int CurrentXp { get; set; }
    public int CurrentPlayerLevel => UpgradeManager.Instance.PointsAllocated / XpPerLevel;
    private int _currentPlayerLevel;
    
    public event Action OnXpChanged;
    public event Action OnPlayerLevelChanged;

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
        CurrentXp = 0;
        _currentPlayerLevel = CurrentPlayerLevel;
        OnPlayerLevelChanged?.Invoke();
        OnXpChanged?.Invoke();
    }

    public void IncreaseXp(int amount)
    {
        CurrentXp += amount;
        OnXpChanged?.Invoke();
        
        CheckPlayerLevelChange();
    }

    public void DecreaseXp(int amount)
    {
        CurrentXp = Mathf.Max(0, CurrentXp - amount);
        OnXpChanged?.Invoke();
        
        CheckPlayerLevelChange();
    }
    
    private void CheckPlayerLevelChange()
    {
        var newLevel = CurrentPlayerLevel;

        if (newLevel == _currentPlayerLevel) return;
        _currentPlayerLevel = newLevel;
        OnPlayerLevelChanged?.Invoke();
    }
}
