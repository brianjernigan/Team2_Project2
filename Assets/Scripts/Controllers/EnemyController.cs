using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;

    public NavMeshAgent NavMeshAgent { get; set; }
    private Transform _playerTransform;
    public bool IsStunned { get; set; }

    public int Health { get; set; }
    public int Damage { get; set; }

    private void InitializeEnemy()
    {
        Health = _enemyData.health;
        Damage = _enemyData.damage;
        NavMeshAgent.speed = _enemyData.speed;
        NavMeshAgent.angularSpeed = _enemyData.angularSpeed;
        NavMeshAgent.stoppingDistance = _enemyData.stoppingDistance;
        NavMeshAgent.acceleration = _enemyData.acceleration;
    }
    
    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
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

    public void DamageEnemy(int amount)
    {
        Health -= amount;
        Debug.Log($"Enemy hit for {amount} damage");

        if (Health <= 0)
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        StatManager.Instance.EnemyDied();
        Destroy(gameObject);
    }
}
