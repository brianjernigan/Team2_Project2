using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerSingleton : MonoBehaviour
{
    public static LevelManagerSingleton Instance { get; private set; }

    private int _totalHouses;
    
    public int HousesVisited { get; set; } = 0;
    public int HousesRemaining => _totalHouses - HousesVisited;
    
    public event Action OnHouseVisited;
    
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeTotalHouses();
    }

    private void InitializeTotalHouses()
    {
        var houses = GameObject.FindGameObjectsWithTag("House");
        _totalHouses = houses.Length;
    }

    public void RegisterHouseVisited()
    {
        HousesVisited++;
        OnHouseVisited?.Invoke();
        
        if (HousesVisited >= _totalHouses)
        {
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        GameManagerSingleton.Instance.LoadNextLevel();
    }
}
