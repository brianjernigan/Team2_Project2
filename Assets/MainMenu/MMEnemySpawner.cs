using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnInterval = 5f;

    private void Start()
    {
        // Start spawning enemies at regular intervals
        InvokeRepeating(nameof(SpawnEnemy), 0f, _spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Spawn the enemy at the spawner's position and rotation
        Instantiate(_enemyPrefab, transform.position, transform.rotation);
    }
}
