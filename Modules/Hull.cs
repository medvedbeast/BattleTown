using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : Module
{
    public float hp;
    public float moveSpeed;

    public override object Clone() => throw new NotImplementedException();

    public override string ToString() => $"Hull\nlvl.{level}";
}
