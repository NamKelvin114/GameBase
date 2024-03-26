using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    public SpecialStats specialStats;
    public int damage;
    public float timeAlive;
    public float coolDown;
    public bool isOneShot;
    public BaseSkill skill;
    public void Init()
    {
        skill.Init(damage, timeAlive, coolDown, isOneShot, specialStats);
    }
}
