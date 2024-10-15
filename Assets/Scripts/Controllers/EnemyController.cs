using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HouseSpawner _spawner;
    
    public void SetSpawner(HouseSpawner houseSpawner)
    {
        _spawner = houseSpawner;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fireball"))
        {
            Destroy(other.gameObject);
            _spawner.KillEnemy(gameObject);
        }
    }
}
