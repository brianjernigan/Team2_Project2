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
    [SerializeField] private ParticleSystem _particles;

    private GameObject _player;
    private HouseSpawner _houseSpawner;
    
    public int XpValue { get; set; }
    public float Damage { get; set; }
    public float Health { get; set; }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        InitializeEnemy();
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
            HurtEnemy(other.gameObject, gameObject);
        }
    }

    private void HurtEnemy(GameObject fireball, GameObject enemy)
    {
        var damageAmount = fireball.GetComponent<FireballController>().Damage;
        Health = Mathf.Max(0, Health - damageAmount);
        AudioManager.Instance.PlayEnemyHitAudio();

        if (!_particles.isPlaying)
        {
            StartCoroutine(FlashParticles());
        }
        else
        {
            StopCoroutine(FlashParticles());
            StartCoroutine(FlashParticles());
        }
        

        Destroy(fireball);
        
        if (Health <= 0)
        {
            KillEnemy(enemy);
        }
    }

    private void KillEnemy(GameObject enemy)
    {
        _houseSpawner.KillEnemy(enemy);
    }

    private IEnumerator FlashParticles()
    {
        _particles.Play();
        yield return new WaitForSeconds(0.15f);
        _particles.Stop();
    }
}
