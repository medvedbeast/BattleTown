using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultHull : Hull
{
    public static DefaultHull LEVEL_1 => new DefaultHull
    {
        level = 1,
        hp = 2,
        moveSpeed = 1
    };

    public static DefaultHull LEVEL_2 => new DefaultHull
    {
        level = 1,
        hp = 4,
        moveSpeed = 0.67f
    };
    public static DefaultHull LEVEL_3 => new DefaultHull
    {
        level = 1,
        hp = 6,
        moveSpeed = 0.5f
    };

    public override object Clone() => new DefaultHull
    {
        level = level,
        hp = hp,
        moveSpeed = moveSpeed
    };
}
