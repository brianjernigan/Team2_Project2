using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushBackController : MonoBehaviour
{
    private const float KnockbackRadius = 5f; // Radius for knockback
    private const float MaxRaycastDistance = 10f; // Max distance enemies will be pushed
    private const float KnockbackDuration = .75f; // Time over which the knockback force will be applied
    private const float KnockbackCooldown = 5f; // Cooldown duration for knockback

    private bool _canKnockback; // Control cooldown
    private float _knockbackCooldownTimer;

    private void Start()
    {
        _canKnockback = true;
    }
    
    private void Update()
    {
        // Manage cooldown
        if (!_canKnockback)
        {
            _knockbackCooldownTimer -= Time.deltaTime;
            if (_knockbackCooldownTimer <= 0f)
            {
                _canKnockback = true;
            }
        }

        // If Q is pressed and knockback is not on cooldown
        if (Input.GetKeyDown(KeyCode.Q) && _canKnockback)
        {
            PerformKnockback();
            _canKnockback = false;
            _knockbackCooldownTimer = KnockbackCooldown; // Start cooldown
        }
    }

    private void PerformKnockback()
    {
        // Find all enemies in knockback radius
        var hitColliders = Physics.OverlapSphere(transform.position, KnockbackRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the object is an enemy
            if (hitCollider.CompareTag("Enemy"))
            {
                // Disable the NavMeshAgent (if available) to stop enemy movement
                var agent = hitCollider.GetComponent<NavMeshAgent>();
                if (agent is not null)
                {
                    agent.enabled = false;
                }

                var direction = (hitCollider.transform.position - transform.position).normalized;

                // Calculate the target position at max raycast distance in the direction
                var targetPosition = transform.position + direction * MaxRaycastDistance;

                // Start a coroutine to apply knockback with an impulse-like effect
                StartCoroutine(MoveEnemyWithImpulse(hitCollider.transform, targetPosition, agent));
            }
        }
    }

    private IEnumerator MoveEnemyWithImpulse(Transform enemyTransform, Vector3 targetPosition,
        NavMeshAgent agent)
    {
        var elapsedTime = 0f;
        var startPosition = enemyTransform.position;

        while (elapsedTime < KnockbackDuration)
        {
            // Calculate the remaining knockback time and the proportion of distance covered
            var t = elapsedTime / KnockbackDuration;

            // Gradually slow down the movement over time (starts fast and slows down)
            enemyTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the enemy reaches the final position
        enemyTransform.position = targetPosition;

        // Once the enemy reaches max range, re-enable the NavMeshAgent
        if (agent is not null)
        {
            agent.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the knockback radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, KnockbackRadius);
    }
}