using Enumerations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hunter : BotController
{
    int wallSearchRadius = 10;

    private Vector3? hidingSpot = null;

    private Vector3 previousAttackTargetPosition;

    private float attackTargetPositionTolerance = 10.0f;
    private float attackTargetPositionCheckTimer = 5.0f;
    private float currentAttackTargetPositionCheckTimer = 0.0f;


    public override Vector3? MoveTarget => hidingSpot;

    public override void Initialize()
    {
        SelectTarget();
        FindHidingSpot();
    }

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
            hidingSpot = null;
        }
    }
    
    private void FindHidingSpot()
    {
        currentAttackTargetPositionCheckTimer = 0.0f;

        var obstacleList = Game.Pathfinder.CellsInRadius(Game.Map, AttackTarget.transform.position, wallSearchRadius, MAP_CELL_TYPE.OBSTACLE);
        var obstacle = obstacleList.OrderByDescending(x => Vector3.Distance(AttackTarget.transform.position, x)).FirstOrDefault();

        var roadList = Game.Pathfinder.CellsInRadius(Game.Map, obstacle, 1, MAP_CELL_TYPE.ROAD);
        var road = roadList.OrderByDescending(x => Vector3.Distance(obstacle, x)).FirstOrDefault();

        if (road != null)
        {
            hidingSpot = new Vector3(road.x, 0, road.z);
        }
    }

    public override void FixedUpdate()
    {
        if (Game.State == GAME_STATE.IN_GAME)
        {
            currentAttackTargetPositionCheckTimer += Time.deltaTime;

            if (AttackTarget != null)
            {
                if (Vector3.Distance(previousAttackTargetPosition, AttackTarget.transform.position) > attackTargetPositionTolerance && currentAttackTargetPositionCheckTimer >= attackTargetPositionCheckTimer)
                {
                    FindHidingSpot();
                }

                previousAttackTargetPosition = new Vector3(AttackTarget.transform.position.x, AttackTarget.transform.position.y, AttackTarget.transform.position.z);
            }
        }

        base.FixedUpdate();
    }
}
