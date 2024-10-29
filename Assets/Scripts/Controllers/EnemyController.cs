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

    private const float DamageScaling = 0.05f;
    private const float HealthScaling = 0.1f;
    private const float SpeedScaling = 0.075f;
    
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
        var playerLevel = XpManager.Instance.CurrentPlayerLevel;

        XpValue = _enemyData.xpValue;

        Damage = _enemyData.damage * (1 + playerLevel * DamageScaling);
        Damage = Mathf.Min(Damage, 10);

        Health = _enemyData.health * (1 + playerLevel * HealthScaling);
        Health = Mathf.Min(Health, 10);

        _navMeshAgent.speed = _enemyData.speed * (1 + playerLevel * SpeedScaling);
    }

    private void Update()
    {
        if (_player is null || _navMeshAgent is null ) return;
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

    //all new in 2 functions below
    public void KillEnemyWithAnimation(GameObject enemy)
    {
        StartCoroutine(PlayDeathAnimationAndKill(enemy));
    }

    // Coroutine to play the death animation and kill the enemy afterward
    private IEnumerator PlayDeathAnimationAndKill(GameObject enemy)
    {
        TriggerDeathAnimation();

        // Wait for the animation to finish 
        yield return new WaitForSeconds(1.0f);

        // Now kill the enemy
        KillEnemy(enemy);
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

    //new referenced from player collisions(can be changed this was just he easiest way to implement this as collisions are handled from the player not the enemy)
    public void TriggerAttackAnimation()
    {
        TryGetComponent<Animator>(out var animator);
        animator?.SetTrigger("attack");
    }

    public void TriggerDeathAnimation()
    {
        TryGetComponent<Animator>(out var animator);
        animator?.SetBool("isDead", true);
    }

}
