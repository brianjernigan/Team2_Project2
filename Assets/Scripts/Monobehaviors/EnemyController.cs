using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _xpPrefab;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    
    private Transform _playerTransform;
    public bool IsStunned { get; set; }
    public float Health { get; set; }
    public float Damage { get; set; }
    public int XpValue { get; set; }

    private EnemySpawner _spawner;
    
    public void InitializeEnemy()
    {
        Health = _enemyData.health;
        Damage = _enemyData.damage;
        XpValue = _enemyData.xpValue;

        if (!NavMeshAgent.enabled)
        {
            NavMeshAgent.enabled = true;
        }
        
        NavMeshAgent.speed = _enemyData.speed;
        NavMeshAgent.angularSpeed = _enemyData.angularSpeed;
        NavMeshAgent.stoppingDistance = _enemyData.stoppingDistance;
        NavMeshAgent.acceleration = _enemyData.acceleration;
        IsStunned = false;
    }

    private void Awake()
    {
        _spawner = FindObjectOfType<EnemySpawner>();
        InitializeEnemy();
    }

    private void Update()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        
        var playerDirection = _playerTransform.position - transform.position;
        playerDirection.y = 0;

        if (playerDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection);
        }
        
        if (_playerTransform is not null && NavMeshAgent is not null && !IsStunned)
        {
            NavMeshAgent.SetDestination(_playerTransform.position);
        }
    }

    public void DamageEnemy(float amount)
    {
        Health -= amount;

        var particles = GetComponentInChildren<ParticleSystem>();
        particles.Play();
        
        if (Health <= 0)
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        StatManager.Instance.EnemyDied();
        _spawner.ReturnEnemyToPool(gameObject);
    }
}
