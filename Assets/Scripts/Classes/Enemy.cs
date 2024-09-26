using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IDamageable
{
    public int Health { get; set; }
    public int Strength { get; set; }

    public Enemy(int health, int strength)
    {
        Health = health;
        Strength = strength;
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
