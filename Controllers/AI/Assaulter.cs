using Enumerations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Assaulter : BotController
{
    public override Vector3? MoveTarget => AttackTarget?.transform.position;

    public override void SelectTarget()
    {
        var enemies = unit.faction == UNIT_FACTION.PLAYER ? Game.Scene.enemies : Game.Scene.allies;
        if (enemies.Count > 0)
        {
            Unit closest = enemies.First();
            var d = Vector3.Distance(transform.position, closest.transform.position);
            foreach (var e in enemies)
            {
                d = Vector3.Distance(transform.position, e.transform.position);
                if (d < Vector3.Distance(transform.position, e.transform.position))
                {
                    closest = e;
                }
            }
            AttackTarget = closest;
        }
    }
}
