using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceRocket : BaseBullet
{
    [SerializeField, Range(0, 10)] private float radiusExplosion;
    [SerializeField] private LayerMask targetLayerMask;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
    protected override void BulletEffect()
    {
        Debug.Log("Explosion");
        var targets = Physics.OverlapSphere(transform.position, radiusExplosion, targetLayerMask).ToList();
        var receiversDame = targets.Select(c => c.GetComponent<ITarget>()).Distinct().ToList();
        var receiversSlow= targets.Select(c => c.GetComponent<ISlow>()).Distinct().ToList();
        foreach (var getEnemyDame in receiversDame)
        {
            getEnemyDame.GetDamage(SpecialStats.damageRange);
        }
        foreach (var getEnemySlow in receiversSlow)
        {
            getEnemySlow.GetSlow(SpecialStats.percentSlowMove,SpecialStats.slowTime);
        }
    }
}
