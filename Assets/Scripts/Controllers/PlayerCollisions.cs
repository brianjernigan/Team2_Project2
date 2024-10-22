using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{ 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other.gameObject);
        }
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

        if (other.CompareTag("BuffCandy") || other.CompareTag("CursedCandy") || other.CompareTag("Candy"))
        {
            HandleCandyCollision(other.gameObject);
        }
    }
    
    private void HandleEnemyCollision(GameObject enemy)
    {
        if (PlayerStatManager.Instance.IsDead) return;
        var damageAmount = enemy.GetComponent<EnemyController>().Damage;
        PlayerStatManager.Instance.DamagePlayer(damageAmount, enemy);
    }

    private void HandleCandyCollision(GameObject candy)
    {
        AudioManager.Instance.PlayCandyCollectAudio();
        LevelManager.Instance.RegisterHouseVisited();
        Destroy(candy);
    }

    private void HandleEnemyBulletCollision(GameObject bullet)
    {
        var damageAmount = bullet.GetComponent<EnemyBulletController>().DamageValue;
        PlayerStatManager.Instance.DamagePlayer(damageAmount, bullet);
    }

    private void HandleXpCollision(GameObject xp)
    {
        var xpAmount = xp.GetComponent<XpController>().XpValue;
        XpManager.Instance.IncreaseXp(xpAmount);
        AudioManager.Instance.PlayXpAudio();
        Destroy(xp);
    }
}
