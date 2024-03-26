using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Skills/BasicShootingSkill")]
public class BasicShootingSkill : BaseSkill
{
    [SerializeField] private EBulletType bulletType;
    public override void DoAttack(ref GameObject target, float aliveTime)
    {
        var bullet = BulletPool.Instance.GetBullet(bulletType);
        bullet.transform.position = center.position;
        bullet.SetActive(true);
        bullet.GetComponent<IBullet>().HitTarget(ref target, damage, target.tag, specialStats,aliveTime);
    }
}
