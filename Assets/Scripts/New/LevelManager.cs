using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private int _totalHouses;
    
    public int HousesVisited { get; set; } = 0;
    public int HousesRemaining => _totalHouses - HousesVisited;
    
    public event Action OnAllHousesVisited;
    
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

        if (HousesVisited >= _totalHouses)
        {
            OnAllHousesVisited?.Invoke();
            HandleLevelCompletion();
        }
    }

    private void HandleLevelCompletion()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
