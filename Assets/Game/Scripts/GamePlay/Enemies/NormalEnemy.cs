using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseEnemy, IExplosion
{
    [SerializeField] private ParticleSystem explosionFX;
    public ParticleSystem ExplosionFX
    {
        get => explosionFX;
        set => explosionFX = value;
    }
    public void DoExplosion(float getDame)
    {
        playerTarget.GetComponent<ITarget>().GetDamage(getDame);
        Observer.CameraShake?.Invoke();
        GetDamage(maxHp);
    }
    public override void DameIncomeAction()
    {

    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
