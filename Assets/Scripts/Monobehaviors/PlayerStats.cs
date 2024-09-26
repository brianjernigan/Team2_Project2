using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public int Strength { get; set; }
    public void TakeDamage()
    {
        Debug.Log("That hurts");
    }
}
