using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _data;

    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;

    public int Health { get; set; }
    public int Damage { get; set; }

    private void InitializeEnemy()
    {
        Health = _data.health;
        Damage = _data.damage;
        _navMeshAgent.speed = _data.speed;
        _navMeshAgent.angularSpeed = _data.angularSpeed;
        _navMeshAgent.stoppingDistance = _data.stoppingDistance;
        _navMeshAgent.acceleration = _data.acceleration;
    }
    
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
        
        if (_playerTransform is not null && _navMeshAgent is not null)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
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

    private void KillEnemy()
    {
        StatManager.Instance.EnemyDied();
        Destroy(gameObject);
    }
}
