using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Support : BotController
{
    private Unit leader;

    public override Vector3? MoveTarget
    {
        get
        {
            if (leader == null)
            {
                SelectLeader();
                if (leader == null)
                {
                    return AttackTarget?.transform.position;
                }
            }
            return leader?.transform.position;
        }
    }

    public override void Initialize()
    {
        SelectTarget();
    }

    public override void SelectTarget()
    {
        if (leader != null)
        {
            var botController = leader.GetComponent<BotController>();
            if (botController != null)
            {
                AttackTarget = leader.GetComponent<BotController>().AttackTarget;
            }
        }

        if (AttackTarget == null)
        {
            var enemies = unit.faction == Enumerations.UNIT_FACTION.PLAYER ? Game.Scene.enemies : Game.Scene.allies;
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

    public void SelectLeader()
    {
        var allies = unit.faction == Enumerations.UNIT_FACTION.PLAYER ? Game.Scene.allies : Game.Scene.enemies;
        allies = allies.Where(x => x != this.unit).ToList();

        if (allies.Count < 1)
        {
            return;
        }

        Unit closest = allies.First();
        var d = Vector3.Distance(transform.position, closest.transform.position);
        foreach (var a in allies)
        {
            d = Vector3.Distance(transform.position, a.transform.position);
            if (d < Vector3.Distance(transform.position, a.transform.position))
            {
                closest = a;
            }
        }
        leader = closest;
    }
}
