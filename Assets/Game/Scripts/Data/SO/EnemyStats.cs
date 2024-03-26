using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float MaxHP;
    public float MaxShield;
    public float moveSpeed;
}
