using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    [SerializeField] private float _maxChainRange = 15f;
    [SerializeField] private float _stunDuration = .5f;
    [SerializeField] private float _pullSpeed = 20f;
    [SerializeField] private float _pushSpeed = 40f;
    [SerializeField] private Transform _player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootChain();
        }
    }

    private void ShootChain()
    {
        var direction = transform.forward;

        // Draw the raycast line for visualization in the Scene view
        Debug.DrawRay(transform.position, direction * _maxChainRange, Color.red, 2f); // Line lasts for 2 seconds

        if (Physics.Raycast(transform.position, direction, out var hit, _maxChainRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit by hook!");
                StartCoroutine(ChainEnemy(hit.collider.gameObject));
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

    private IEnumerator ChainEnemy(GameObject enemy)
    {
        // Step 1: Pull enemy to the max hook range
        var direction = (enemy.transform.position - _player.position).normalized;
        var maxRangePosition = _player.position + direction * _maxChainRange;

        while (Vector3.Distance(enemy.transform.position, maxRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, maxRangePosition, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        // Stun enemy for initial stun duration once they get to max range
        StunEnemy(enemy, _stunDuration);
        yield return new WaitForSeconds(_stunDuration);

        // Step 2: Pull enemy to 2 units in front of the player
        var closeRangePosition = _player.position + (enemy.transform.position - _player.position).normalized * 2f;

        while (Vector3.Distance(enemy.transform.position, closeRangePosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, closeRangePosition, _pullSpeed * Time.deltaTime);
            yield return null;
        }

        // Stun enemy again for double the initial stun duration
        StunEnemy(enemy, _stunDuration * 2);
        yield return new WaitForSeconds(_stunDuration * 2);

        // Kill (destroy) the enemy after the second stun duration
        enemy.GetComponent<EnemyController>().KillEnemy();
    }

    private void StunEnemy(GameObject enemy, float duration)
    {
        // Example: disabling enemy's movement or actions
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController?.Stun(duration);
    }


}
