using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTurret : Turret
{
    public static DefaultTurret LEVEL_1 => new DefaultTurret
    {
        level = 1,
        accuracy = 0.7f
    };

    public static DefaultTurret LEVEL_2 => new DefaultTurret
    {
        level = 2,
        accuracy = 0.8f
    };

    public static DefaultTurret LEVEL_3 => new DefaultTurret
    {
        level = 3,
        accuracy = 0.9f
    };

    public DefaultTurret()
    {
        turnSpeed = 1.0f;
    }

    public override object Clone() => new DefaultTurret
    {
        level = level,
        accuracy = accuracy,
        turnSpeed = turnSpeed
    };
}
