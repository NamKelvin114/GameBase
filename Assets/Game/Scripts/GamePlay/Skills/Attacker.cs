using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Attacker : BaseAttack
{
    [SerializeField, Range(1, 100)] private float rangeAttack;
    [SerializeField] protected List<GameObject> targetList;
    [SerializeField] protected SkillGroup skillGroup;
    private void Update()
    {
        if (target == null)
        {
            var checkTarget = GameObject.FindGameObjectsWithTag(Constant.Enemy);
            targetList.Clear();
            foreach (var getTar in checkTarget)
            {
                if (!targetList.Contains(getTar))
                {
                    targetList.Add(getTar);
                }
            }
            foreach (var targetCloset in targetList)
            {
                if (Vector3.Distance(center.position, targetCloset.transform.position) <= rangeAttack)
                {
                    target = targetCloset;
                    break;
                }
            }
        }
        else
        {
            if (Vector3.Distance(center.position, target.transform.position) > rangeAttack)
            {
                target = null;
                return;
            }
            if (currentSkill==null)
            {
                InitToAttack(skillGroup.skillDatas[0].skill);
            }
            CalCoolDown();
        }
    }
    // private void CalCoolDown()
    // {
    //     _currentSkill.TempCoolDown -= Time.deltaTime;
    //     if (_currentSkill.TempCoolDown <= 0)
    //     {
    //         if (_currentSkill.IsOneShot)
    //         {
    //             _currentSkill.Attack(ref target,center);
    //             _currentSkill.TempCoolDown = _currentSkill.CoolDown;
    //         }
    //         else
    //         {
    //             _currentSkill.TempTimeAlive -= Time.deltaTime;
    //             if (_currentSkill.TempTimeAlive > 0)
    //             {
    //                 _currentSkill.Attack(ref target,center);
    //             }
    //             else
    //             {
    //                 _currentSkill.EndAliveSkill();
    //                 _currentSkill.TempCoolDown = _currentSkill.CoolDown;
    //                 _currentSkill.TempTimeAlive = _currentSkill.TimeAlive;
    //             }
    //         }
    //     }
    // }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.position, rangeAttack);
    }
}
