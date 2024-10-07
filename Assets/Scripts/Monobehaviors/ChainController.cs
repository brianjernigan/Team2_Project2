using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChainController : MonoBehaviour
{
    private const float MaxChainRange = 15f;
    private const float StunDuration = .5f;
    private const float PullSpeed = 20f;
    private const float PushSpeed = 40f;
    [SerializeField] private Transform _player;

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

        if (Physics.Raycast(transform.position, direction, out var hit, MaxChainRange))
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
        AudioManager.Instance.PlayChainAudio();
        
        var enemyController = enemy.GetComponent<EnemyController>();
        
        enemyController.IsStunned = true;
        enemyController.NavMeshAgent.enabled = false;
        
        // Step 1: Push enemy to the max hook range
        var direction = (enemy.transform.position - _player.position).normalized;
        var maxRangePosition = _player.position + direction * MaxChainRange;

        while (Vector3.Distance(enemy.transform.position, maxRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, maxRangePosition, PushSpeed * Time.deltaTime);
            yield return null;
        }

        // Stun enemy for initial stun duration once they get to max range
        StunEnemy(StunDuration, enemyController);
        yield return new WaitForSeconds(StunDuration);

        // Step 2: Pull enemy to 2 units in front of the player
        var closeRangePosition = _player.position + (enemy.transform.position - _player.position).normalized * 2f;

        while (Vector3.Distance(enemy.transform.position, closeRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, closeRangePosition, PullSpeed * Time.deltaTime);
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
