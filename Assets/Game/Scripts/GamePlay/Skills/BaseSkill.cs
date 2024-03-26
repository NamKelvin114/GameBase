using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseSkill : ScriptableObject, IAttack
{
    protected int damage;
    protected SpecialStats specialStats;
    public float TimeAlive
    {
        get => timeAlive;
        set => timeAlive = value;
    }
    public float CoolDown
    {
        get => coolDown;
        set => coolDown = value;
    }
    public bool IsOneShot
    {
        get => isOneShot;
        set => isOneShot = value;
    }
    protected GameObject attackTo;
    protected Transform center;
    private float timeAlive;
    private float coolDown;
    private bool isOneShot;
    public void Init(int getDamage, float getTime, float getCoolDown, bool getIsOneShot, SpecialStats getSpecialStats)
    {
        specialStats = getSpecialStats;
        damage = getDamage;
        timeAlive = getTime;
        coolDown = getCoolDown;
        isOneShot = getIsOneShot;
    }

    public void Attack(ref GameObject getTarget, Transform getCenter)
    {
        center = getCenter;
        attackTo = getTarget;
        DoAttack(ref getTarget,timeAlive);
    }
    public virtual void EndAliveSkill()
    {

    }
    public abstract void DoAttack(ref GameObject target, float aliveTime);
}
