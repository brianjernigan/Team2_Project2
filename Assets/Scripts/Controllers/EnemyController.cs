using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private GameObject _player;
    private HouseSpawner _houseSpawner;
    
    public int XpValue { get; set; }
    public float Damage { get; set; }
    public float Health { get; set; }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        
    }

    private void InitializeEnemy()
    {
        XpValue = Random.Range(1, _enemyData.xpValue);
        Damage = _enemyData.damage;
        Health = _enemyData.health;
    }

    private void Update()
    {
        if (_player is null || _navMeshAgent is null) return;
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    
    public void SetSpawner(HouseSpawner houseSpawner)
    {
        _houseSpawner = houseSpawner;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fireball"))
        {
            Destroy(other.gameObject);
            _houseSpawner.KillEnemy(gameObject);
        }
    }
}
