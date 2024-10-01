using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushBackController : MonoBehaviour
{
    private float knockbackRadius = 7.5f;  // Radius to detect enemies
    private float knockbackForce = 750f;   // Force to apply to enemies
    public LayerMask enemyLayer;          // Layer of the enemies
    private Rigidbody enemyRigidbody;
    private bool isKnockBack;

    void Update()
    {
        // Check if the Q key is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Trigger knockback at the player's position
            TriggerKnockback(transform.position);
        }

    }

    // Call this method to trigger the knockback
    public void TriggerKnockback(Vector3 origin)
    {
        // Create a sphere cast to find all enemies in the radius
        Collider[] hitColliders = Physics.OverlapSphere(origin, knockbackRadius, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            enemyRigidbody = hitCollider.gameObject.GetComponent<Rigidbody>();
            var navMeshAgent = hitCollider.gameObject.GetComponent<NavMeshAgent>();

            if (enemyRigidbody != null && navMeshAgent is not null)
            {
                navMeshAgent.isStopped = true; // Stop the agent temporarily

                var backwardsDirection = -enemyRigidbody.gameObject.transform.forward;

                // Apply knockback force
                enemyRigidbody.AddForce(backwardsDirection * knockbackForce, ForceMode.Impulse);


                StartCoroutine(KnockbackEffect(enemyRigidbody, navMeshAgent));
            }
        }
    }

    private IEnumerator KnockbackEffect(Rigidbody erb, NavMeshAgent nma)
    {
        while (erb.drag <= 1f)
        {
            erb.drag += 0.01f;
            yield return null;
        }

        erb.drag = 0.1f;
        nma.isStopped = false; // Resume the NavMeshAgent
    }

    // Optional: Visualize the sphere cast in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
