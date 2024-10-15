using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [Header("Enemy Components")] 
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Fence & Gate")] 
    [SerializeField] private ParticleSystem _gateParticles;
    [SerializeField] private Animator _gateAnimator;
    [SerializeField] private GameObject _exitTrigger;

    [Header("XP")] 
    [SerializeField] private GameObject _xpPrefab;

    private int _enemiesPerWave = 2;
    private int _totalWaves = 2;
    private float _spawnInterval = 0.5f;

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
            HandleEndOfHouse();
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

        var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        var enemyController = enemy.GetComponent<EnemyController>();

        enemyController?.SetSpawner(this);
    }

    public void KillEnemy(GameObject enemy)
    {
        _enemiesRemaining--;
        Instantiate(_xpPrefab, enemy.transform.position, enemy.transform.rotation);
        Destroy(enemy);

        if (_enemiesRemaining > 0) return;
        // Next wave
        _currentWave++;
        StartCoroutine(SpawnNextWave());
    }

    private void HandleEndOfHouse()
    {
        AudioManagerSingleton.Instance.PlayDoorCreakAudio();
        _gateAnimator.SetTrigger("openGate");
        _gateParticles.Play();
        LevelManagerSingleton.Instance.RegisterHouseVisited();
        _exitTrigger.SetActive(true);
    }
}
