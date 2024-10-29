using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HouseSpawner : MonoBehaviour
{
    [Header("Enemy Components")] 
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Fence & Gate")] 
    [SerializeField] private Animator _gateAnimator;

    [Header("XP")] 
    [SerializeField] private GameObject _xpPrefab;
    [SerializeField] private GameObject _candyPrefab;
    [SerializeField] private Transform _candySpawnPoint;

    [SerializeField] private RingDoorbellUIController _ringDoorbellUIController;

    private const float BaseSpawnInterval = 0.75f;
    private const int BaseEnemiesPerWave = 2;
    private const int BaseTotalWaves = 2;
    private const float BaseTimeBetweenWaves = 3f;
    
    public int CurrentEnemiesPerWave { get; set; }
    public int CurrentTotalWaves { get; set; }
    public float CurrentTimeBetweenWaves { get; set; }
    public float CurrentSpawnInterval { get; set; }

    private int _currentWave;
    private int _enemiesRemaining;

    private int _houseIndex;
    private int _previousEnemyIndex = -1;
    private int _previousSpawnPointIndex = -1;

    private void OnEnable()
    {
        XpManager.Instance.OnXpChanged += UpdateHouseStats;
    }

    private void OnDisable()
    {
        if (XpManager.Instance is not null)
        {
            XpManager.Instance.OnXpChanged -= UpdateHouseStats;
        }
    }

    private void InitializeHouseStats()
    {
        UpdateHouseStats();
        CurrentTimeBetweenWaves = BaseTimeBetweenWaves;
        CurrentSpawnInterval = BaseSpawnInterval;
    }

    private void UpdateHouseStats()
    {
        var xpCollected = XpManager.Instance.XpCollected;

        CurrentEnemiesPerWave = BaseEnemiesPerWave + (xpCollected / 5);
        CurrentTotalWaves = BaseTotalWaves + (xpCollected / 10);
    }

    public void StartSpawning()
    {
        InitializeHouseStats();
        StartCoroutine(SpawnNextWave());
    }

    private IEnumerator SpawnNextWave()
    {
        if (_currentWave >= CurrentTotalWaves)
        {
            HandleEndOfHouse();
            yield break;
        }
        
        if (_currentWave > 0)
        {
            StartCoroutine(NextWaveRoutine());
            yield return new WaitForSeconds(CurrentTimeBetweenWaves);
            _ringDoorbellUIController.HideWaveText();
        }

        _enemiesRemaining = CurrentEnemiesPerWave;

        for (var i = 0; i < CurrentEnemiesPerWave; i++)
        {
            yield return StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(BaseSpawnInterval);
        }
    }

    private IEnumerator NextWaveRoutine()
    {
        _ringDoorbellUIController.ShowWaveText("Wave Complete!");
        yield return new WaitForSeconds(1f);
        _ringDoorbellUIController.ShowWaveText($"Next round in {CurrentTimeBetweenWaves} seconds");
    }

    private IEnumerator SpawnEnemy()
    {
        var enemyIndex = GetUniqueEnemyIndex();
        var spawnPointIndex = GetUniqueSpawnPointIndex();

        var spawnPoint = _spawnPoints[spawnPointIndex];
        var particles = spawnPoint.GetComponent<ParticleSystem>();

        particles?.Play();
        yield return new WaitForSeconds(1.0f);
        particles?.Stop();

        var enemy = Instantiate(_enemyPrefabs[enemyIndex], _spawnPoints[spawnPointIndex].position,
            _spawnPoints[spawnPointIndex].rotation);

        _previousEnemyIndex = enemyIndex;
        _previousSpawnPointIndex = spawnPointIndex;
        
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController?.SetSpawner(this);
    }

    private int GetUniqueEnemyIndex()
    {
        int enemyIndex;
        do
        {
            enemyIndex = Random.Range(0, _enemyPrefabs.Length);
        } while (enemyIndex == _previousEnemyIndex && _enemyPrefabs.Length > 1);

        return enemyIndex;
    }

    private int GetUniqueSpawnPointIndex()
    {
        int spawnPointIndex;
        do
        {
            spawnPointIndex = Random.Range(0, _spawnPoints.Length);
        } while (spawnPointIndex == _previousSpawnPointIndex && _spawnPoints.Length > 1);

        return spawnPointIndex;
    }

    public void KillEnemy(GameObject enemy)
    {
        _enemiesRemaining--;
            
        var xp = Instantiate(_xpPrefab, enemy.transform.position + Vector3.up, enemy.transform.rotation);
        // Set Xp Value in XpController
        xp.GetComponent<XpController>().XpValue = enemy.GetComponent<EnemyController>().XpValue;
        Destroy(enemy);
        
        if (_enemiesRemaining > 0) return;
        _currentWave++;
        StartCoroutine(SpawnNextWave());
    }

    private void HandleEndOfHouse()
    {
        AudioManager.Instance.PlayDoorCreakAudio();
        _gateAnimator.SetTrigger("openGate");
        var candy = Instantiate(_candyPrefab, _candySpawnPoint.position, _candySpawnPoint.rotation);
    }
}
