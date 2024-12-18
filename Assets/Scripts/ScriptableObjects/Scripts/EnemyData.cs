using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyType type;
    public int xpValue;
    public float health;
    public float damage;
    public float speed;
}
