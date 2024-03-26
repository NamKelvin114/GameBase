using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Test2 : MonoBehaviour
{
    public string valueTest;
    public virtual void Printed()
    {
        Debug.Log("helanam");
    }
    public abstract void JJ();
}
