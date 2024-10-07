using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChainController : MonoBehaviour
{
    [SerializeField] private float _maxChainRange = 15f;
    [SerializeField] private float _stunDuration = .5f;
    [SerializeField] private float _pullSpeed = 20f;
    [SerializeField] private float _pushSpeed = 40f;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioManager _audio;

    private bool _canTrigger;
    public float ChainCooldown { get; private set; } = 10f;

    public event Action OnChainTriggered;

    private void Start()
    {
        _canTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canTrigger)
        {
            ShootChain();
        }
    }

    private void ShootChain()
    {
        var direction = transform.forward;

        if (Physics.Raycast(transform.position, direction, out var hit, _maxChainRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit by hook!");
                StartCoroutine(ChainEnemy(hit.collider.gameObject));
                _canTrigger = false;
                StartCoroutine(CooldownRoutine());
                OnChainTriggered?.Invoke();
            }
            else
            {
                Debug.Log("Hook hit: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Hook missed.");
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(ChainCooldown);
        _canTrigger = true;
    }

    private IEnumerator ChainEnemy(GameObject enemy)
    {
        _audio.PlayChainAudio();
        
        var enemyController = enemy.GetComponent<EnemyController>();
        
        enemyController.IsStunned = true;
        enemyController.NavMeshAgent.enabled = false;
        
        // Step 1: Push enemy to the max hook range
        var direction = (enemy.transform.position - _player.position).normalized;
        var maxRangePosition = _player.position + direction * _maxChainRange;

        while (Vector3.Distance(enemy.transform.position, maxRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, maxRangePosition, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        // Stun enemy for initial stun duration once they get to max range
        StunEnemy(_stunDuration, enemyController);
        yield return new WaitForSeconds(_stunDuration);

        // Step 2: Pull enemy to 2 units in front of the player
        var closeRangePosition = _player.position + (enemy.transform.position - _player.position).normalized * 2f;

        while (Vector3.Distance(enemy.transform.position, closeRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, closeRangePosition, _pullSpeed * Time.deltaTime);
            yield return null;
        }

        // Stun enemy again for double the initial stun duration
        // StunEnemy(_stunDuration * 2, enemyController);
        // yield return new WaitForSeconds(_stunDuration * 2);

        // Kill (destroy) the enemy after the second stun duration
        enemyController.KillEnemy();
    }

    private void StunEnemy(float duration, EnemyController ec)
    {
        if (ec.IsStunned) return;
        StartCoroutine(RemoveStun(duration, ec));
    }

    private IEnumerator RemoveStun(float duration, EnemyController ec)
    {
        yield return new WaitForSeconds(duration);
        ec.IsStunned = false;
        ec.NavMeshAgent.enabled = true;
    }
}
