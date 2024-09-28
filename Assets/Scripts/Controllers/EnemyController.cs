using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData _data;
    
    public void TakeDamage()
    {
        Debug.Log("Enemy takes damage");
    }
}
