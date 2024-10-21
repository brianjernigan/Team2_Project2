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
    private Animator _animator; //new

    private const float AdditionalDamagePerLevel = 2f;
    private const float AdditionalHealthPerLevel = 2f;
    
    public int XpValue { get; set; }
    public float Damage { get; set; }
    public float Health { get; set; }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>(); //new
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        var playerLevel = XpManager.Instance.CurrentPlayerLevel;
        
        XpValue = Random.Range(1, _enemyData.xpValue);
        Damage = Mathf.Min(_enemyData.damage + (playerLevel - 1) * AdditionalDamagePerLevel, 20);
        Health = Mathf.Min(_enemyData.health + (playerLevel - 1) * AdditionalHealthPerLevel, 20);
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

    //all new in 2 functions below
    public void KillEnemyWithAnimation(GameObject enemy)
    {
        StartCoroutine(PlayDeathAnimationAndKill(enemy));
    }

    // Coroutine to play the death animation and kill the enemy afterward
    private IEnumerator PlayDeathAnimationAndKill(GameObject enemy)
    {
        // Play the death animation 
        _animator.SetBool("isDead", true);

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
    public void AttackAnimation()
    {
        _animator?.SetTrigger("attack");
    }
}
