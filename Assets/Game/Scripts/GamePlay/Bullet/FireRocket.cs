using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pancake.OdinSerializer.Utilities;
using UnityEngine;

public class FireRocket : BaseBullet
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
        var receivers = targets.Select(c => c.GetComponent<ITarget>()).Distinct().ToList();
        Debug.Log(receivers.Count);
        foreach (var getEnemy in receivers)
        {
            getEnemy.GetDamage(SpecialStats.damageRange);
        }
    }
}
