using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    [SerializeField] protected Transform center;
    [SerializeField] protected GameObject target;
    private float _tempCoolDown;
    private float _tempTimeAlive;
    protected BaseSkill currentSkill;
    protected void InitToAttack(BaseSkill getcurrentSkill)
    {
        currentSkill = getcurrentSkill;
        _tempCoolDown = currentSkill.CoolDown;
        _tempTimeAlive = currentSkill.TimeAlive;
    }
    protected void CalCoolDown()
    {
        _tempCoolDown -= Time.deltaTime;
        if (_tempCoolDown <= 0)
        {
            if (currentSkill.IsOneShot)
            {
                currentSkill.Attack(ref target, center);
                _tempCoolDown = currentSkill.CoolDown;
            }
            else
            {
                if (_tempTimeAlive==currentSkill.TimeAlive)
                {
                    currentSkill.Attack(ref target, center);
                }
                _tempTimeAlive -= Time.deltaTime;
                if (_tempTimeAlive > 0)
                {
                    return;
                }
                currentSkill.EndAliveSkill();
                _tempCoolDown = currentSkill.CoolDown;
                _tempTimeAlive = currentSkill.TimeAlive;
            }
        }
    }
}
