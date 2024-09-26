using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public int Strength { get; set; }
    
    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
