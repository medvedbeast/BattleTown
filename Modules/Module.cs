using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : ICloneable
{
    public int level;

    public virtual bool IsUpdateable => false;

    public virtual void Update() { }

    public virtual void Initialize(GameObject target) { }

    public virtual object Clone() => throw new NotImplementedException();
}
