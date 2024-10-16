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
        var damageAmount = enemy.GetComponent<EnemyController>().Damage;
        PlayerStatManagerSingleton.Instance.DamagePlayer(damageAmount);
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

        if (other.CompareTag("HouseExitTrigger"))
        {
            HandleHouseExit(other.gameObject);
        }
    }

    private void HandleEnemyBulletCollision(GameObject bullet)
    {
        var damageAmount = bullet.GetComponent<EnemyBulletController>().DamageValue;
        PlayerStatManagerSingleton.Instance.DamagePlayer(damageAmount);
    }

    private void HandleXpCollision(GameObject xp)
    {
        var xpAmount = xp.GetComponent<XpController>().XpValue;
        ExperienceManagerSingleton.Instance.IncreaseXp(xpAmount);
        AudioManagerSingleton.Instance.PlayXpAudio();
        Destroy(xp);
    }

    private void HandleHouseExit(GameObject trigger)
    {
        Debug.Log("exiting house");
        LevelManagerSingleton.Instance.RegisterHouseVisited();
        trigger.SetActive(false);
    }
}
