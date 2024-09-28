using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerTransform;

    private int _initialPoolSize;

    private ObjectPool<GameObject> _enemyPool;

    private float _spawnInterval = 3f;

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

        InvokeRepeating(nameof(SpawnEnemy), 0, _spawnInterval);
    }

    private void SpawnEnemy()
    {
        var enemy = _enemyPool.Get();
    }

    private GameObject CreatePooledEnemy()
    {
        var enemy = Instantiate(_enemyPrefab);
        enemy.SetActive(false);
        return enemy;
    }

    private void OnTakeFromPool(GameObject enemy)
    {
        var spawnPoint = GetRandomPointOnNavMesh();

        spawnPoint.y += 1.045f;
        enemy.transform.position = spawnPoint;

        var playerDirection = _playerTransform.position - spawnPoint;
        playerDirection.y = 0;

        if (playerDirection != Vector3.zero)
        {
            enemy.transform.rotation = Quaternion.LookRotation(playerDirection);
        }
        
        enemy.SetActive(true);
    }

    private void OnReturnedToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject enemy)
    {
        Destroy(enemy);
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        _enemyPool.Release(enemy);
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
            "L1" => 40,
            "L2" => 60,
            "L3" => 80,
            _ => 1
        };
    }
}
