using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotController : UnitController
{
    public float aimRange;
    public float aimTimer;
    public float currentAimTimer;
    [Space(15)]
    public float movementTolerance = 0.25f;
    public float pathfindingTolerance = 2.5f;
    public float rotationTolerance = 5;
    [Space(15)]
    public List<Vector3> path = new List<Vector3>();
    public float pathfindingTimer = 0.5f;
    public float currentPathfindingTimer = 0.0f;

    public virtual Unit AttackTarget { get; set; }
    public virtual Vector3 PreviousMoveTarget { get; set; }
    public virtual Vector3? MoveTarget { get; set; }

    public virtual void Initialize() { }

    public virtual void SelectTarget() { }

    public override void Start()
    {
        base.Start();

        turnSpeedModifier = 5.0f;
        aimRange = unit.weapon.range;

        Initialize();
    }

    public virtual void Pathfind()
    {
        PreviousMoveTarget = MoveTarget.Value;
        currentPathfindingTimer = 0;
        path.Clear();

        var from = Game.Pathfinder.GetClosestEmptyPoint(Game.Map, transform.position);
        var to = Game.Pathfinder.GetClosestEmptyPoint(Game.Map, MoveTarget.Value);
        if (from.HasValue && to.HasValue)
        {
            var result = Game.Pathfinder.Search(Game.Map, new Vector2(from.Value.x, from.Value.z), new Vector2(to.Value.x, to.Value.z));
            foreach (var p in result)
            {
                path.Add(new Vector3(p.x, 0, p.y));
            }
        }

        path.Remove(path.First());
    }

    public virtual void FixedUpdate()
    {
        if (Game.State == Enumerations.GAME_STATE.IN_GAME)
        {
            currentPathfindingTimer += Time.deltaTime;

            var hasAttackTargetInSight = false;

            if (AttackTarget != null)
            {
                if (Vector3.Distance(transform.position, AttackTarget.transform.position) <= aimRange)
                {
                    hasAttackTargetInSight = Aim();
                    if (hasAttackTargetInSight)
                    {
                        if (currentAimTimer >= aimTimer)
                        {
                            currentAimTimer = 0;
                            aimTimer = Random.Range(0, 101) * 0.01f;
                            Shoot();
                        }
                        else
                        {
                            currentAimTimer += Time.deltaTime;
                        }

                    }
                }
            }
            else
            {
                SelectTarget();
            }

            if (MoveTarget != null)
            {
                if (Vector3.Distance(PreviousMoveTarget, MoveTarget.Value) > pathfindingTolerance && currentPathfindingTimer >= pathfindingTimer)
                {
                    Pathfind();
                }

                if (!hasAttackTargetInSight)
                {
                    Move();
                }
            }

            if (unit.interactiveObjects.Count > 0 && unit.faction == Enumerations.UNIT_FACTION.ENEMY)
            {
                var interactive = new List<Interactive>(unit.interactiveObjects);
                foreach (var i in interactive)
                {
                    if (i is Crate)
                    {
                        var c = i as Crate;
                        var owned = unit.modules.FirstOrDefault(x => x.GetType() == c.module.GetType());
                        if (owned != null && owned.level < c.module.level)
                        {
                            c.Use(unit);
                        }
                        else
                        {
                            c.Destroy();
                        }
                    }
                }
            }
        }
    }

    public virtual void Move()
    {
        if (path.Count == 0)
        {
            return;
        }

        var coordinates = path.First();
        // adjust for correct tank position
        var waypoint = new Vector3(coordinates.x + 0.5f, coordinates.y, coordinates.z + 0.5f);

        if (Vector3.Distance(transform.position, waypoint) <= movementTolerance)
        {
            path.Remove(coordinates);
            Move();
            return;
        }
        else
        {
            if (RotateTo(new Vector3(waypoint.x, transform.position.y, waypoint.z)))
            {
                rigidbody.AddForce(transform.forward * moveSpeedModifier * unit.MoveSpeed);
            }
        }
    }

    public virtual bool RotateTo(Vector3 point)
    {
        var direction = point - transform.position;

        if (Vector3.Angle(direction, transform.forward) > rotationTolerance)
        {
            var step = turnSpeedModifier * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual bool Aim()
    {
        var point = AttackTarget.transform.position;
        point.y = unit.turretObject.transform.position.y;

        var targetRotation = Quaternion.LookRotation(point - unit.turretObject.transform.position);
        unit.turretObject.transform.rotation = Quaternion.Slerp(unit.turretObject.transform.rotation, targetRotation, Time.deltaTime * turretTurnSpeedModifier * unit.TurretTurnSpeed);

        Debug.DrawRay(transform.position, unit.turretObject.transform.forward, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(unit.turretObject.transform.position, unit.turretObject.transform.forward, out hit, 10.0f))
        {
            if (hit.collider.gameObject == AttackTarget.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void Shoot()
    {
        unit.weapon.Shoot(gameObject);
    }

    public virtual void OnTargetDestroyed()
    {
    }
}
