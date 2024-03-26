using System.Collections;
using System.Collections.Generic;
using Pancake;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Skills/LazerSkill")]
public class LazerSkill : BaseSkill
{

    public override void DoAttack(ref GameObject target, float aliveTime)
    {
        var lazerBullet = BulletPool.Instance.GetBullet(EBulletType.Lazer);
        lazerBullet.SetActive(true);
        lazerBullet.GetComponent<LazerBullet>().Init(center);
        lazerBullet.GetComponent<IBullet>().HitTarget(ref target, damage, target.tag, specialStats, aliveTime);
        // lazerBullet.transform.position = center.position;
        // var dir = target.transform.position - center.transform.position;
        // lazerBullet.transform.forward = dir;
        
    }
}
