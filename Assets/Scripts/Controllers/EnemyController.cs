using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;

    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;
    private bool _isStunned;

    public int Health { get; set; }
    public int Damage { get; set; }

    private void InitializeEnemy()
    {
        _isStunned = false;
        Health = _enemyData.health;
        Damage = _enemyData.damage;
        _navMeshAgent.speed = _enemyData.speed;
        _navMeshAgent.angularSpeed = _enemyData.angularSpeed;
        _navMeshAgent.stoppingDistance = _enemyData.stoppingDistance;
        _navMeshAgent.acceleration = _enemyData.acceleration;
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
        
        if (_playerTransform is not null && _navMeshAgent is not null && !_isStunned)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
    }

    public void Stun(float duration)
    {
        if (_isStunned) return;
        // Add stun logic here (e.g., disable movement)
        StartCoroutine(RemoveStun(duration));
    }

    private IEnumerator RemoveStun(float duration)
    {
        _isStunned = true;
        
        _navMeshAgent.enabled = false;
        
        yield return new WaitForSeconds(duration);
        
        _isStunned = false;
        _navMeshAgent.enabled = true;
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
