using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Skills/SpecialStats")]
public class SpecialStats : ScriptableObject
{
    public float damageRange;
    public float slowTime;
    [Range(0, 100)] public float percentSlowMove;
}
