using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Module
{
    public float accuracy;
    public float turnSpeed;

    public override object Clone() => throw new NotImplementedException();

    public override string ToString() => $"Turret\nlvl.{level}";
}
