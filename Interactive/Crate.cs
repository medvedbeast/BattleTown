using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crate : Interactive
{
    public Module module;

    public override void Use(Unit u)
    {
        u.Mount(module);
        Destroy();
    }

    public override void Destroy()
    {
        foreach (var i in interactors)
        {
            i.OnInteractionLeave(this);
        }

        Destroy(gameObject);
    }

}
