using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : BaseEnemy
{
    public override void DameIncomeAction()
    {
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
