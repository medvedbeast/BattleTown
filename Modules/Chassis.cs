using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : Module
{
    public float turnSpeed;
    public float reverseSpeed;

    public override object Clone() => throw new NotImplementedException();

    public override string ToString() => $"Chassis\nlvl.{level}";
}
