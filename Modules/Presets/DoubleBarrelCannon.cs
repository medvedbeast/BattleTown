using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelCannon : Weapon
{
    public static DoubleBarrelCannon LEVEL_1 => new DoubleBarrelCannon
    {
        level = 1,
        damage = 1,
        range = 3
    };

    public static DoubleBarrelCannon LEVEL_2 => new DoubleBarrelCannon
    {
        level = 2,
        damage = 2,
        range = 4
    };

    public static DoubleBarrelCannon LEVEL_3 => new DoubleBarrelCannon
    {
        level = 3,
        damage = 3,
        range = 5
    };

    private GameObject gunParent;

    public DoubleBarrelCannon()
    {
        hitEffectName = "Projectile Hit 1";
        projectileName = "Projectile 2";
        rpm = 30;
    }

    public override void Initialize(GameObject target)
    {
        barrels = new List<GameObject>
        {
            target.transform.Find("Turret/Gun/Gun (DB)/L").gameObject,
            target.transform.Find("Turret/Gun/Gun (DB)/R").gameObject
        };
        gunParent = target.transform.Find("Turret/Gun/Gun (DB)").gameObject;

        HideAllWeapons(target);

        gunParent.SetActive(true);
    }

    public override object Clone() => new DoubleBarrelCannon
    {
        level = level,
        barrels = barrels?.Count > 0 ? new List<GameObject>(barrels) : new List<GameObject>(),
        damage = damage,
        hitEffectName = hitEffectName,
        projectileName = projectileName,
        range = range,
        rechargeTime = rechargeTime,
        rpm = rpm
    };

    public override string ToString() => $"DB Gun\nlvl.{level}";
}
