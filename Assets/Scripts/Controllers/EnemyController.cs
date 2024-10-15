using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    private HouseSpawner _houseSpawner;
    
    public int XpValue { get; set; }
    public float Damage { get; set; }

    private void Awake()
    {
        XpValue = Random.Range(1, _enemyData.xpValue);
        Damage = _enemyData.damage;
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
