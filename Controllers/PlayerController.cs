using Enumerations;
using System.Linq;
using UnityEngine;

public class PlayerController : UnitController
{
    private float interactionTimer = 0.5f;
    private float currentInteractionTimer = 0.0f;

    void FixedUpdate()
    {
        if (Game.State == GAME_STATE.IN_GAME)
        {
            currentInteractionTimer += Time.deltaTime;

            acceleration = Input.GetAxis("Vertical");
            acceleration *= acceleration < 0 ? unit.MoveSpeed * unit.ReverseSpeed : unit.MoveSpeed;
            sideAcceleration = Input.GetAxisRaw("Horizontal");

            Move();
            Rotate();
            RotateTurret();

            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            if (unit.interactiveObjects.Count > 0 && currentInteractionTimer >= interactionTimer)
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    var i = unit.interactiveObjects.Last();
                    i.Use(unit);

                    currentInteractionTimer = 0;
                }
                else if (Input.GetKey(KeyCode.M))
                {
                    var i = unit.interactiveObjects.Last();
                    i.Destroy();

                    currentInteractionTimer = 0;
                }
            }
        }

    }

    private void Move()
    {
        rigidbody.AddForce(transform.forward * acceleration * moveSpeedModifier * unit.MoveSpeed);
    }

    private void Rotate()
    {
        rigidbody.AddTorque(transform.up * sideAcceleration * turnSpeedModifier * unit.TurnSpeed);
    }

    private void RotateTurret()
    {
        var turret = unit.turretObject.transform.position;
        var camera = Camera.main;
        
        var point = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        point.y = turret.y;

        var direction = point - turret;
        var newDirection = Vector3.RotateTowards(unit.turretObject.transform.forward, direction, Time.deltaTime * turretTurnSpeedModifier * unit.TurretTurnSpeed, 0.0f);

        unit.turretObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void Shoot()
    {
        unit.weapon.Shoot(gameObject);
    }
}
