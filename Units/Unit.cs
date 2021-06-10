using Enumerations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Space(15)]
    public float hp;
    public float maxHp;
    public bool penetrable = false;
    public bool isDesturcted = false;
    [Space(15)]
    public float outputDamageMultiplier = 1.0f;
    public float inputDamageMultiplier = 1.0f;
    public float armor = 0.0f;
    [Space(15)]
    public float xp = 0.0f;
    public float maxXp = 100.0f;
    public int level = 1;
    public float value = 50.0f;
    [Space(15)]
    public UNIT_FACTION faction;
    public UNIT_TYPE type;
    [Space(15)]
    public GameObject turretObject;
    [Space(15)]
    public float interactionDistance = 1.5f;

    public float HealthInPercent => hp / (maxHp * 0.01f);
    public float MoveSpeed => hull.moveSpeed;
    public float TurnSpeed => chassis.turnSpeed;
    public float ReverseSpeed => chassis.reverseSpeed;
    public float TurretTurnSpeed => turret.turnSpeed;

    public Hull hull;
    public Chassis chassis;
    public Turret turret;
    public Weapon weapon;
    
    public event System.Action<Unit> HpChange;
    public event System.Action<Unit> XpChange;
    public event System.Action<Unit> EquipmentChange;
    public event System.Action<Unit> Destruction;
    public event System.Action<Unit, Interactive> InteractionStart;
    public event System.Action<Unit, Interactive> InteractionEnd;

    public List<Module> modules;
    public List<Interactive> interactiveObjects;

    public Unit()
    {
        interactiveObjects = new List<Interactive>();
        modules = new List<Module>();

        Destruction += OnDestruction;
    }

    public void Start()
    {
        HpChange?.Invoke(this);   
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i].IsUpdateable)
            {
                modules[i].Update();
            }
        }
    }

    public void Clone(Unit u)
    {
        modules = u.modules;
        faction = u.faction;

        EquipmentChange?.Invoke(this);
    }

    public void Mount(Module m)
    {
        var installed = modules.Where(x => x.GetType().BaseType == m.GetType().BaseType).ToList();
        if (installed.Count > 0)
        {
            foreach (var i in installed)
            {
                modules.Remove(i);
            }
        }

        m.Initialize(gameObject);
        modules.Add(m);

        OnEquipmentChange();

        EquipmentChange?.Invoke(this);
    }

    public void Mount(List<Module> list)
    {
        foreach (var m in list)
        {
            var installed = modules.Where(x => x.GetType().BaseType == m.GetType().BaseType).ToList();
            if (installed.Count > 0)
            {
                foreach (var i in installed)
                {
                    modules.Remove(i);
                }
            }

            m.Initialize(gameObject);
            modules.Add(m);
        }
        OnEquipmentChange();

        EquipmentChange?.Invoke(this);
    }

    // events

    public void OnEquipmentChange(Unit unit = null)
    {
        hull = modules.FirstOrDefault(x => x is Hull) as Hull;
        chassis = modules.FirstOrDefault(x => x is Chassis) as Chassis;
        turret = modules.FirstOrDefault(x => x is Turret) as Turret;
        weapon = modules.FirstOrDefault(x => x is Weapon) as Weapon;


        if (maxHp != 0)
        {
            var hpPercent = hp / (maxHp * 0.01f);
            maxHp = hull.hp;
            hp = maxHp * (hpPercent * 0.01f);
        }
        else
        {
            maxHp = hull.hp;
            hp = maxHp;
        }


        HpChange?.Invoke(this);
    }

    public void OnKill(Unit u)
    {
        xp += u.value;
        while (xp > maxXp)
        {
            xp -= maxXp;
            level++;
            maxXp *= 1.15f;

            // amplify stats after a levelup (optional)
            /*
            outputDamageMultiplier += 0.25f;
            inputDamageMultiplier -= 0.25f;
            */
        }
        XpChange?.Invoke(this);
    }

    public void OnDestruction(Unit _)
    {
        var drop = modules.Where(x => x.level > 1 && !(x is Chassis)).ToList();
        if (drop.Count > 0)
        {
            var index = Random.Range(0, drop.Count);
            var item = drop[index];

            Game.SpawnCrate(transform.position, item.Clone() as Module);
        }

        Destroy(gameObject);
    }

    public void OnInteractionEnter(Interactive i)
    {
        interactiveObjects.Add(i);
        InteractionStart?.Invoke(this, i);
    }

    public void OnInteractionLeave(Interactive i)
    {
        interactiveObjects.Remove(i);
        InteractionEnd?.Invoke(this, i);
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
            Projectile projectile = c.GetComponent<Projectile>();

            if (projectile.owner != null)
            {
                Unit shooter = projectile.owner.GetComponent<Unit>();

                if (!isDesturcted && shooter != null)
                {
                    if (shooter == this)
                    {
                        return;
                    }

                    Weapon weapon = projectile.cannon;
                    var damage = (weapon.damage * shooter.outputDamageMultiplier) * (Mathf.Clamp((1 - armor * 0.01f) * inputDamageMultiplier, 0, 1));

                    var chance = shooter.turret.accuracy;
                    var isHit = Random.Range(1, 101) * 0.01f <= chance ? true : false;

                    if (isHit)
                    {
                        hp -= damage;
                        HpChange?.Invoke(this);
                        Debug.Log($"{name} hit! {hp} hp left");
                    }
                    else
                    {
                        Debug.Log("Miss!");
                    }

                    if (hp <= 0)
                    {
                        GameObject effect = Instantiate(Resources.Load("Prefabs/Particles/Explosion 1")) as GameObject;
                        effect.transform.position = c.transform.position;
                        effect.transform.parent = GameObject.Find("Effects").transform;

                        isDesturcted = true;

                        Destruction?.Invoke(this);
                    }
                }
            }    
            

            if (!projectile.penetrative || !this.penetrable)
            {
                GameObject effect = Instantiate(Resources.Load($"Prefabs/Particles/{projectile.cannon.hitEffectName}")) as GameObject;
                effect.transform.position = c.transform.position;
                effect.transform.parent = GameObject.Find("Effects").transform;

                Destroy(c.gameObject);
            }

        }
    }
}
