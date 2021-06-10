using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : BotController
{
    public override Vector3? MoveTarget => AttackTarget?.transform.position;

    public override void SelectTarget()
    {
        AttackTarget = unit.faction == Enumerations.UNIT_FACTION.ENEMY ? Game.Scene.alliedBase : Game.Scene.enemyBase;
    }

}
