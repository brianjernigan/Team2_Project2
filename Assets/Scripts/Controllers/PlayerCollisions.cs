using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{ 
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other.gameObject);
        }
    }
    
    private void HandleEnemyCollision(GameObject enemy)
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            HandleEnemyBulletCollision(other.gameObject);
        }

        if (other.CompareTag("XP"))
        {
            HandleXpCollision(other.gameObject);
        }
    }

    private void HandleEnemyBulletCollision(GameObject bullet)
    {
        throw new NotImplementedException();
    }

    private void HandleXpCollision(GameObject xp)
    {
        throw new NotImplementedException();
    }
}
