using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public void DoAttack(ref GameObject target, float aliveTime);
}
