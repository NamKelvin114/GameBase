using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassMono : MonoBehaviour
{
    public ClassnoMono ClassnoMono;
    public List<ClassnoMono1> ClassnoMono1s;
    private void Start()
    {
        ClassnoMono = new ClassnoMono();
        ClassnoMono.classes = new List<ClassnoMono1>();
        ClassnoMono.classes.Add(new ClassnoMono1());
        Debug.Log(ClassnoMono.classes[0].a);
    }
}
