using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    protected Unit unit;
    protected new Rigidbody rigidbody;
    protected float acceleration;
    protected float sideAcceleration;

    public float moveSpeedModifier = 3000.0f;
    public float turnSpeedModifier = 250.0f;
    public float turretTurnSpeedModifier = 5.0f;

    public virtual void Start()
    {
        unit = GetComponent<Unit>();
        unit.turretObject = gameObject.transform.Find("Turret").gameObject;
        
        rigidbody = GetComponent<Rigidbody>();
    }
}
