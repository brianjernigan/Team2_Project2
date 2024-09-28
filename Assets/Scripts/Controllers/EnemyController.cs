using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData _data;

    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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

    public void TakeDamage()
    {
        Debug.Log("Enemy takes damage");
    }
}
