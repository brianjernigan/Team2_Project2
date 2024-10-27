using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMEnemyMovementScript : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    void Update()
    {
        // Move the enemy forward endlessly
        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the enemy collided with the death zone
        if (other.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}
