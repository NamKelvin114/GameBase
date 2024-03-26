using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Skills/ExploreSkill")]
public class ExploreSkill : BaseSkill
{
    public override void DoAttack(ref GameObject target, float aliveTime)
    {
        var ex = center.gameObject.GetComponentInParent<IExplosion>();
        var Fx = ex.ExplosionFX;
        var getFX = Instantiate(Fx, center.position, quaternion.identity);
        center.gameObject.Destroy();
        ex.DoExplosion(damage);
        getFX.Play();
        var mainFX = getFX.main;
        DOTween.Sequence().AppendInterval(mainFX.duration).AppendCallback(() =>
        {
            getFX.Stop();
            Destroy(getFX.gameObject);
        });
    }
}
