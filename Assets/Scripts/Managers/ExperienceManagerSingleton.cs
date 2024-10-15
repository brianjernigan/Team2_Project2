using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManagerSingleton : MonoBehaviour
{
    public static ExperienceManagerSingleton Instance { get; private set; }

    private const int BaseXpThreshold = 10;
    private const float XpMultiplier = 1.55f;
    
    public int CurrentXp { get; private set; }
    public int CurrentPlayerLevel { get; private set; }
    public int CurrentXpThreshold { get; private set; }

    public event Action OnXpChanged;
    public event Action OnPlayerLevelUp;

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

    public void InitializeXp()
    {
        CurrentPlayerLevel = 1;
        CurrentXp = 0;
        CurrentXpThreshold = BaseXpThreshold;
        OnXpChanged?.Invoke();
    }

    public void IncreaseXp(int amount)
    {
        CurrentXp += amount;
        OnXpChanged?.Invoke();

        if (CurrentXp >= CurrentXpThreshold)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentPlayerLevel++;
        CurrentXpThreshold = Mathf.RoundToInt(BaseXpThreshold * Mathf.Pow(XpMultiplier, CurrentPlayerLevel - 1));
        OnPlayerLevelUp?.Invoke();
    }
}
