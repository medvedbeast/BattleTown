using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGame : View
{
    [Header("Base HP Bar")]
    [Space(5)]
    public UnityEngine.UI.Text alliedBaseHpBarText;
    public UnityEngine.UI.Image alliedBaseHpBarFill;
    [Space(5)]
    public UnityEngine.UI.Text enemyBaseHpBarText;
    public UnityEngine.UI.Image enemyBaseHpBarFill;
    [Space(15)]
    [Header("Spawn counter")]
    [Space(5)]
    public UnityEngine.UI.Text alliedSpawnCountText;
    [Space(5)]
    public UnityEngine.UI.Text enemySpawnCountText;
    [Space(15)]
    [Header("Aiming")]
    [Space(5)]
    public UnityEngine.UI.Image aimingSight;
    public UnityEngine.UI.Image actualAimingPoint;
    [Space(15)]
    [Header("Tooltip")]
    [Space(5)]
    public UnityEngine.UI.Image tooltip;
    public UnityEngine.UI.Text tooltipText;
    [Space(15)]
    [Header("Equipment")]
    [Space(5)]
    public UnityEngine.UI.Text gunLevelText;
    public UnityEngine.UI.Text turretLevelText;
    public UnityEngine.UI.Text hullLevelText;
    public UnityEngine.UI.Text chassisLevelText;
    [Space(15)]
    [Header("Health bars")]
    [Space(5)]
    public GameObject healthBarsAnchor;

    private Transform tooltipTarget;
    private Dictionary<Unit, GameObject> healthBarTargets = new Dictionary<Unit, GameObject>();

    public void FixedUpdate()
    {
        if (Game.State == Enumerations.GAME_STATE.IN_GAME)
        {
            aimingSight.transform.position = Input.mousePosition;

            var u = Game.Scene.player;
            var turretPosition = u.turretObject.transform.position;
            var range = u.weapon.range;
            RaycastHit hit;
            if (Physics.Raycast(turretPosition, Game.Scene.player.turretObject.transform.forward, out hit, range, LayerMask.GetMask("Scene")))
            {
                var p = Camera.main.WorldToScreenPoint(hit.point);
                actualAimingPoint.transform.position = p;
            }
            else
            {
                var p = Camera.main.WorldToScreenPoint(turretPosition + u.turretObject.transform.forward * range);
                actualAimingPoint.transform.position = p;
            }

            if (tooltip.transform.gameObject.activeSelf && tooltipTarget != null)
            {
                tooltip.transform.position = Camera.main.WorldToScreenPoint(tooltipTarget.position + new Vector3(0, 0, 0));
            }

            foreach (var t in healthBarTargets)
            {
                t.Value.transform.position = Camera.main.WorldToScreenPoint(t.Key.transform.position + new Vector3(0.0f, 0.5f, 0.5f));
            }
        }
    }

    public void OnBaseHealthChange(Unit b)
    {
        var amount = b.HealthInPercent * 0.01f;
        if (b == Game.Scene.alliedBase)
        {
            alliedBaseHpBarFill.fillAmount = amount > 0 ? amount : 0.0f;
            alliedBaseHpBarText.text = $"{b.hp} / {b.maxHp}";
        }
        else
        {
            enemyBaseHpBarFill.fillAmount = amount > 0 ? amount : 0.0f;
            enemyBaseHpBarText.text = $"{b.hp} / {b.maxHp}";
        }
    }

    public void OnUnitSpawn(Unit unit)
    {
        alliedSpawnCountText.text = (Game.Instance.maxSpawnCount - Game.Instance.alliesSpawned).ToString();
        enemySpawnCountText.text = (Game.Instance.maxSpawnCount - Game.Instance.enemiesSpawned).ToString();

        if (unit != null)
        {
            var prefab = GameObject.Instantiate(Resources.Load("Prefabs/UI/HP Bar")) as GameObject;
            prefab.transform.SetParent(healthBarsAnchor.transform);
            prefab.transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + new Vector3(0.0f, 0.5f, 0.5f));
            prefab.transform.localScale = new Vector3(1, 1, 1);
            healthBarTargets.Add(unit, prefab);
            unit.Destruction += OnUnitDestruction;
        }
    }

    public void OnUnitDestruction(Unit unit)
    {
        if (healthBarTargets.ContainsKey(unit))
        {
            GameObject.Destroy(healthBarTargets[unit]);
            healthBarTargets.Remove(unit);
        }

    }

    public void OnUnitHealthChange(Unit unit)
    {
        if (healthBarTargets.ContainsKey(unit))
        {
            healthBarTargets[unit].transform.Find("HP").GetComponent<UnityEngine.UI.Image>().fillAmount = unit.HealthInPercent * 0.01f;
        }
    }

    public void OnInteractionStart(Unit u, Interactive i)
    {
        if (i is Crate && u == Game.Scene.player)
        {
            var c = i as Crate;
            tooltipText.text = c.module.ToString();
            tooltip.transform.gameObject.SetActive(true);
            tooltipTarget = i.transform;
            tooltip.transform.position = Camera.main.WorldToScreenPoint(tooltipTarget.position + new Vector3(0, 0, 0));
        }
    }

    public void OnInteractionEnd(Unit u, Interactive i)
    {
        if (u == Game.Scene.player)
        {
            tooltip.transform.gameObject.SetActive(false);
            tooltipTarget = null;
        }
    }

    public void OnUnitEquipmentChange(Unit u)
    {
        if (u == Game.Scene.player)
        {
            gunLevelText.text = u.weapon.level.ToString();
            turretLevelText.text = u.turret.level.ToString();
            hullLevelText.text = u.hull.level.ToString();
            chassisLevelText.text = u.chassis.level.ToString();
        }
    }
}
