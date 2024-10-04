using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private GameObject _xpPrefab;

    private int _initialPoolSize;
    private ObjectPool<GameObject> _enemyPool;

    private const float BaseSpawnRate = 2.5f;
    private const float MinSpawnRate = 0.5f;
    private const float SpawnRateScale = 0.975f;
    
    private const float XpHeight = 1.045f;

    private void Awake()
    {
        _initialPoolSize = GetPoolSize();
    }

    private void Start()
    {
        _enemyPool = new ObjectPool<GameObject>(
            CreatePooledEnemy,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            _initialPoolSize,
            100
        );

        InvokeRepeating(nameof(SpawnEnemy), 0,
            Mathf.Max(MinSpawnRate, BaseSpawnRate * Mathf.Pow(SpawnRateScale, StatManager.Instance.CurrentPlayerLevel - 1)));
    }

    private void SpawnEnemy()
    {
        var enemy = _enemyPool.Get();
    }

    private GameObject CreatePooledEnemy()
    {
        var enemyIndex = Random.Range(0, _enemyPrefabs.Count);
        var enemy = Instantiate(_enemyPrefabs[enemyIndex]);
        enemy.SetActive(false);
        return enemy;
    }

    private void OnTakeFromPool(GameObject enemy)
    {
        var spawnPoint = GetRandomPointOnNavMesh();
        spawnPoint.y += 1.045f;
        enemy.transform.position = spawnPoint;
        enemy.SetActive(true);

        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController?.InitializeEnemy();
    }

    private void OnReturnedToPool(GameObject enemy)
    {
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController?.InitializeEnemy();
        enemy.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject enemy)
    {
        Destroy(enemy);
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        if (!enemy.activeSelf) return;
        DropXp(enemy);
        _enemyPool.Release(enemy);
    }
    
    private void DropXp(GameObject enemy)
    {
        var targetPos = enemy.transform.position;
        targetPos.y = XpHeight;
        var xp = Instantiate(_xpPrefab, targetPos, Quaternion.identity);

        SetXpValue(xp, enemy);
    }

    private void SetXpValue(GameObject xp, GameObject enemy)
    {
        var xpController = xp.GetComponent<XpController>();
        var xpAmount = enemy.GetComponent<EnemyController>().XpValue;

        xpController.Value = xpAmount;
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        var navMeshData = NavMesh.CalculateTriangulation();
        var randomTriangleIndex = Random.Range(0, navMeshData.indices.Length / 3) * 3;

        var vertex1 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex]];
        var vertex2 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex + 1]];
        var vertex3 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex + 2]];

        return GetRandomPointInTriangle(vertex1, vertex2, vertex3);
    }

    private Vector3 GetRandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        var r1 = Mathf.Sqrt(Random.value);
        var r2 = Random.value;

        return (1 - r1) * a + (r1 * (1 - r2)) * b + (r1 * r2) * c;
    }

    private int GetPoolSize()
    {
        return SceneManager.GetActiveScene().name switch
        {
            "L1" => 25,
            "L2" => 50,
            "L3" => 75,
            _ => 100
        };
    }
}
