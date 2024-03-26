using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public EBulletType BulletType { get; set; }
    public GameObject Target { get; set; }
    public bool IsUsing { set; get; }
    public float DameIncome { get; set; }
    public SpecialStats SpecialStats{get; set; }
    public void HitTarget(ref GameObject target, float getDameIncome,string targetTag,SpecialStats specialStats, float getTimeAlive);
}
