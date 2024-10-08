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

    private const float HealthScale = 1.025f;
    private const float DamageScale = 1.05f;
    private const float SpeedScale = 1.075f;
    
    public void InitializeEnemyStats()
    {
        Health = _enemyData.health * Mathf.Pow(HealthScale, StatManager.Instance.CurrentPlayerLevel - 1);
        Damage = _enemyData.damage * Mathf.Pow(DamageScale, StatManager.Instance.CurrentPlayerLevel - 1);
        XpValue = _enemyData.xpValue;

        if (!NavMeshAgent.enabled)
        {
            NavMeshAgent.enabled = true;
        }

        NavMeshAgent.speed = _enemyData.speed * Mathf.Pow(SpeedScale, StatManager.Instance.CurrentPlayerLevel - 1);
        NavMeshAgent.angularSpeed = _enemyData.angularSpeed;
        NavMeshAgent.stoppingDistance = _enemyData.stoppingDistance;
        NavMeshAgent.acceleration = _enemyData.acceleration;
        IsStunned = false;
    }

    private void Awake()
    {
        _spawner = FindObjectOfType<EnemySpawner>();
        InitializeEnemyStats();
    }

    private void Start()
    {
        NavMeshAgent.updateRotation = true;
    }

    private void Update()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        
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
