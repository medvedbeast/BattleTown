using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPenetratingCannon : Weapon
{
    public static ArmorPenetratingCannon LEVEL_1 => new ArmorPenetratingCannon
    {
        level = 1,
        damage = 2,
        range = 4
    };

    public static ArmorPenetratingCannon LEVEL_2 => new ArmorPenetratingCannon
    {
        level = 2,
        damage = 4,
        range = 6
    };

    public static ArmorPenetratingCannon LEVEL_3 => new ArmorPenetratingCannon
    {
        level = 3,
        damage = 6,
        range = 8
    };

    public ArmorPenetratingCannon()
    {
        hitEffectName = "Projectile Hit 1";
        projectileName = "Projectile 1";
        rpm = 30;
    }

    public override void Initialize(GameObject target)
    {
        barrels = new List<GameObject>
        {
            target.transform.Find("Turret/Gun/Gun (AP)").gameObject
        };

        HideAllWeapons(target);

        barrels[0].SetActive(true);
    }

    public override object Clone() => new ArmorPenetratingCannon
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

    public override string ToString() => $"AP Gun\nlvl.{level}";
}
