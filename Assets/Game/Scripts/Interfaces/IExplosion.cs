using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosion
{
    public ParticleSystem ExplosionFX { get; set; }
    public void DoExplosion(float getDame);
}
