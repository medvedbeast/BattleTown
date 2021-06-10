using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Module
{
    public float range;
    public float rpm;
    public float damage;

    public List<GameObject> barrels;

    public string projectileName = "";
    public string hitEffectName = "";

    public override bool IsUpdateable => true;

    protected float rechargeTime = 0.0f;

    public override void Update()
    {
        if (rechargeTime > 0)
        {
            rechargeTime -= Time.deltaTime;
        }
    }

    public void Shoot(GameObject owner)
    {
        if (rechargeTime <= 0)
        {
            foreach (var b in barrels)
            {
                string name = projectileName.Length > 0 ? projectileName : "Projectile 1";
                GameObject p = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/" + name)) as GameObject;
                p.name = name;

                Projectile projectile = p.GetComponent<Projectile>();
                projectile.owner = owner;
                projectile.cannon = this;
                projectile.startPosition = b.transform.position;
                projectile.effectiveRange = range;

                p.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, b.transform.position.z);

                p.transform.Rotate(0, b.transform.parent.transform.rotation.eulerAngles.y, 0);
                p.transform.SetParent(Game.Scene.projectiles.transform);

                p.GetComponent<Rigidbody>().AddForce(b.transform.forward * projectile.speed);
            }

            rechargeTime = 60 / rpm;
        }
    }

    public void HideAllWeapons(GameObject target)
    {
        foreach (Transform t in target.transform.Find("Turret/Gun"))
        {
            t.gameObject.SetActive(false);
        }
    }

    public override object Clone() => throw new NotImplementedException();
}
