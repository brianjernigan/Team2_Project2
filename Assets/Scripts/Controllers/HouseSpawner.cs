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

    [Header("XP")] 
    [SerializeField] private GameObject _xpPrefab;
    [SerializeField] private GameObject _candyPrefab;
    [SerializeField] private Transform _candySpawnPoint;

    private const float BaseSpawnInterval = 0.75f;
    private const int BaseEnemiesPerWave = 3;
    private const int BaseTotalWaves = 1;
    private const float BaseTimeBetweenWaves = 3f;
    private const int AdditionalEnemiesPerLevel = 2;
    private const int AdditionalWavesPerXLevels = 5;
    
    public int CurrentEnemiesPerWave { get; set; }
    public int CurrentTotalWaves { get; set; }
    public float CurrentTimeBetweenWaves { get; set; }
    public float CurrentSpawnInterval { get; set; }

    private int _currentWave;
    private int _enemiesRemaining;

    private int _houseIndex;
    private int _previousEnemyIndex = -1;
    private int _previousSpawnPointIndex = -1;

    private void InitializeHouseStats()
    {
        var playerLevel = XpManager.Instance.CurrentPlayerLevel;

        CurrentEnemiesPerWave = Mathf.Min(BaseEnemiesPerWave + playerLevel * AdditionalEnemiesPerLevel, 20);
        CurrentTotalWaves = Mathf.Min(BaseTotalWaves + playerLevel / AdditionalWavesPerXLevels, 5);
        CurrentTimeBetweenWaves = BaseTimeBetweenWaves;
        CurrentSpawnInterval = BaseSpawnInterval;
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
            yield return new WaitForSeconds(CurrentTimeBetweenWaves);
        }

        _enemiesRemaining = CurrentEnemiesPerWave;

        for (var i = 0; i < CurrentEnemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(BaseSpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        var enemyIndex = GetUniqueEnemyIndex();
        var spawnPointIndex = GetUniqueSpawnPointIndex();

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
        
        var xp = Instantiate(_xpPrefab, enemy.transform.position, enemy.transform.rotation);
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
