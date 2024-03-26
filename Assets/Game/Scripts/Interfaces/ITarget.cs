using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITarget
{
    public float HP { get; set; }
    public float Energy { get; set; }
    public float Shield { get; set; }
    public void GetDamage(float damage);
    public bool IsDead { get; set; }
}
