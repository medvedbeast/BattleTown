using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultChassis : Chassis
{
    public static DefaultChassis LEVEL_1 => new DefaultChassis
    {
        level = 1
    };

    public DefaultChassis()
    {
        turnSpeed = 1.0f;
        reverseSpeed = 0.5f;
    }

    public override object Clone() => new DefaultChassis
    {
        level = level,
        reverseSpeed = reverseSpeed,
        turnSpeed = turnSpeed
    };
}
