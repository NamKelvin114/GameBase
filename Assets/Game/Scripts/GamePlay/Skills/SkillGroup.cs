using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/SkillGroup")]
public class SkillGroup : ScriptableObject
{
    public List<SkillData> skillDatas;
    public ETypeSkillGroup skillGroupType;
    public void SetupSkillData()
    {
        foreach (var skill in skillDatas)
        {
            skill.Init();
        }
    }
}
public enum ETypeSkillGroup
{
    NormalShooting,
    Explore,
    ShootingLight,
    FireRocket,
    IceRocket,
    Slash,
    Lazer,
}
