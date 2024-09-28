using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    private int _initialPoolSize;

    private List<GameObject> _enemyPool;

    private void Awake()
    {
        _initialPoolSize = GetPoolSize();
    }

    private int GetPoolSize()
    {
        return SceneManager.GetActiveScene().name switch
        {
            "L1" => 10,
            "L2" => 20,
            "L3" => 30,
            _ => 1
        };
    }
}
