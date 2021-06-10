using Enumerations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [Header("Root objects")]
    [Space(5)]
    public GameObject logic;
    public GameObject map;
    public GameObject units;
    public GameObject buildings;
    public GameObject effects;
    public GameObject interactive;
    public GameObject projectiles;
    public GameObject decorations;
    public GameObject ui;

    [Space(15)]
    [Header("Units")]
    [Space(5)]
    public Unit player;
    public Unit alliedBase;
    public Unit enemyBase;
    public List<Unit> enemies = new List<Unit>();
    public List<Unit> allies = new List<Unit>();
}

