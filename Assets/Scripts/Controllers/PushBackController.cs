using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushBackController : MonoBehaviour
{
    private const float KnockbackRadius = 7.5f; // Radius for knockback
    private const float KnockbackDuration = .75f; // Time over which the knockback force will be applied
    private const float KnockbackCooldown = 6f; // Cooldown duration for knockback

    public AudioClip shockWaveSound;
    public Image cooldownBar; // UI image for cooldown bar

    private AudioSource _audioSource;
    private bool _canKnockback; // Control cooldown
    private float _knockbackCooldownTimer;

    private void Start()
    {
        _canKnockback = true;
        _knockbackCooldownTimer = 0f;

        _audioSource = gameObject.AddComponent<AudioSource>();
        if (shockWaveSound != null)
        {
            _audioSource.clip = shockWaveSound;
        }

        if (cooldownBar != null)
        {
            cooldownBar.fillAmount = 1; // Full bar at start
        }
    }

    private void Update()
    {
        // Manage cooldown
        if (!_canKnockback)
        {
            _knockbackCooldownTimer -= Time.deltaTime;

            // Update UI bar if assigned
            if (cooldownBar != null)
            {
                cooldownBar.fillAmount = 1 - (_knockbackCooldownTimer / KnockbackCooldown);
            }

            if (_knockbackCooldownTimer <= 0f)
            {
                _canKnockback = true;
                if (cooldownBar != null)
                {
                    cooldownBar.fillAmount = 1; // Reset cooldown bar
                }
            }
        }

        // If Space is pressed and knockback is not on cooldown
        if (Input.GetKeyDown(KeyCode.Space) && _canKnockback)
        {
            PerformKnockback();
            _canKnockback = false;
            _knockbackCooldownTimer = KnockbackCooldown; // Start cooldown
        }
    }

    private void PerformKnockback()
    {
        Debug.Log("Knockback triggered");

        // Play shockwave sound
        if (_audioSource != null && shockWaveSound != null)
        {
            _audioSource.Play();
        }

        // Find all enemies in knockback radius
        var hitColliders = Physics.OverlapSphere(transform.position, KnockbackRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the object is an enemy
            if (hitCollider.CompareTag("Enemy"))
            {
                // Get the Rigidbody component
                var rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    var direction = (hitCollider.transform.position - transform.position).normalized;
                    // Calculate the target position based on knockback distance
                    var targetPosition = transform.position + direction * KnockbackRadius;

                    StartCoroutine(MoveEnemyWithRigidbody(rb, targetPosition));
                }
            }
        }
    }

    private IEnumerator MoveEnemyWithRigidbody(Rigidbody enemyRigidbody, Vector3 targetPosition)
    {
        var elapsedTime = 0f;

        // Start position of the enemy
        var startPosition = enemyRigidbody.position;

        while (elapsedTime < KnockbackDuration)
        {
            // Move the enemy towards the target position
            enemyRigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / KnockbackDuration));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the enemy reaches the final position
        enemyRigidbody.MovePosition(targetPosition);
    }
}
