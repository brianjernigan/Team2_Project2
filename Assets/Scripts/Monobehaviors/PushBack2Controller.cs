using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushBack2Controller : MonoBehaviour
{
    [SerializeField] private float _knockbackRadius = 5f;       // Radius for knockback
    [SerializeField] private float _initialKnockbackForce = 100f; // Initial force applied for knockback
    [SerializeField] private float _maxRaycastDistance = 10f;   // Max distance enemies will be pushed
    [SerializeField] private float _knockbackDuration = .75f;     // Time over which the knockback force will be applied
    [SerializeField] private float _knockbackCooldown = 5f;     // Cooldown duration for knockback

    private bool _canKnockback = true; // Control cooldown
private float _knockbackCooldownTimer = 0f;

void Update()
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
        _knockbackCooldownTimer = _knockbackCooldown; // Start cooldown
    }
}

private void PerformKnockback()
{
    // Find all enemies in knockback radius
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _knockbackRadius);

    foreach (Collider hitCollider in hitColliders)
    {
        // Check if the object is an enemy
        if (hitCollider.CompareTag("Enemy"))
        {
            // Disable the NavMeshAgent (if available) to stop enemy movement
            NavMeshAgent agent = hitCollider.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }

            Vector3 direction = (hitCollider.transform.position - transform.position).normalized;

            // Calculate the target position at max raycast distance in the direction
            Vector3 targetPosition = transform.position + direction * _maxRaycastDistance;

            // Start a coroutine to apply knockback with an impulse-like effect
            StartCoroutine(MoveEnemyWithImpulse(hitCollider.transform, targetPosition, agent));
        }
    }
}

private System.Collections.IEnumerator MoveEnemyWithImpulse(Transform enemyTransform, Vector3 targetPosition, NavMeshAgent agent)
{
    float elapsedTime = 0f;
    Vector3 startPosition = enemyTransform.position;

    while (elapsedTime < _knockbackDuration)
    {
        // Calculate the remaining knockback time and the proportion of distance covered
        float t = elapsedTime / _knockbackDuration;

        // Gradually slow down the movement over time (starts fast and slows down)
        enemyTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

        elapsedTime += Time.deltaTime;

        yield return null; // Wait for the next frame
    }

    // Ensure the enemy reaches the final position
    enemyTransform.position = targetPosition;

    // Once the enemy reaches max range, re-enable the NavMeshAgent
    if (agent != null)
    {
        agent.enabled = true;
    }
}

private void OnDrawGizmos()
{
    // Visualize the knockback radius in the editor
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _knockbackRadius);
}
}
