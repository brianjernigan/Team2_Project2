using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData _data;

    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;
    
    public void TakeDamage()
    {
        Debug.Log("Enemy takes damage");
    }
}
