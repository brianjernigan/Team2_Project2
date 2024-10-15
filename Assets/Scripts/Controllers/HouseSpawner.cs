using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [Header("Enemy Components")] 
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Fence & Gate")] 
    [SerializeField] private Animator _gateAnimator;

    private int _enemiesPerWave = 2;
    private int _totalWaves = 2;
    private float _spawnInterval = 1f;

    private int _currentWave;
    private int _enemiesRemaining;

    private int _houseIndex;

    public void StartSpawning()
    {
        StartCoroutine(SpawnNextWave());
    }

    private IEnumerator SpawnNextWave()
    {
        if (_currentWave >= _totalWaves)
        {
            yield break;
        }

        _enemiesRemaining = _enemiesPerWave;

        for (var i = 0; i < _enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        var enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
