using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private int _totalHouses;
    
    public int HousesVisited { get; set; }
    public int HousesRemaining => _totalHouses - HousesVisited;
    
    public event Action OnHouseVisited;
    
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
        InitializeTotalHouses();
    }

    public void InitializeTotalHouses()
    {
        var houses = GameObject.FindGameObjectsWithTag("House");
        _totalHouses = houses.Length;
        OnHouseVisited?.Invoke();
    }

    public void RegisterHouseVisited()
    {
        HousesVisited++;
        OnHouseVisited?.Invoke();
        
        if (HousesVisited >= _totalHouses)
        {
            GameManager.Instance.OnPlayerVictory();
        }
    }
}
