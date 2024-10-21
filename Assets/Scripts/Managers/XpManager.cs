using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager Instance { get; private set; }

    private const int XpPerLevel = 10;
    
    public int XpCollected { get; set; }
    public int CurrentXp { get; set; }
    public int CurrentPlayerLevel => (int)MathF.Max(1, (int)Math.Ceiling((double)XpCollected / XpPerLevel));

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

    private void Update()
    {
        Debug.Log(CurrentPlayerLevel);
    }

    public void InitializeXp()
    {
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
