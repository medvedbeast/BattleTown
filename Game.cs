using Enumerations;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //

    public static event System.Action<Unit> UnitSpawn;
    public static event System.Action GameStateChange;

    // fields

    [Header("Spawn controls")]
    [Space(5)]
    public int maxArmySize;
    public int alliesSpawned;
    public int enemiesSpawned;
    public int maxSpawnCount;

    private GAME_STATE state;
    private EventController events;
    private UserInterfaceController ui;
    private Scene scene;
    private Randomizer.Controller randomizer = new Randomizer.Controller();
    private Map map;

    // static fields


    public static GAME_STATE State
    {
        get => instance.state;
        protected set
        {
            instance.state = value;
            GameStateChange?.Invoke();
        }
    }
    public static Scene Scene => instance.scene;
    public static Map Map => instance.map;
    public static Pathfinder Pathfinder => Pathfinder.Instance;
    public static EventController Events => instance.events;
    public static UserInterfaceController UI => instance.ui;
    public static Game Instance => instance;

    private static Game instance;

    // static methods

    public static List<Unit> GetAllies(UNIT_FACTION faction) => faction == UNIT_FACTION.PLAYER ? Scene.allies : Scene.enemies;
    public static Unit GetAlliedBase(UNIT_FACTION faction) => faction == UNIT_FACTION.PLAYER ? Scene.alliedBase : Scene.enemyBase;

    public static void SpawnUnit(string name, UNIT_FACTION faction, bool isPlayerControlled = false, System.Type botBehaviourType = null)
    {
        // prefab

        var position = Pathfinder.GetEmptySpawnPoint(Map, faction);
        var prefabName = faction == UNIT_FACTION.PLAYER ? "Prefabs/Tank 2" : "Prefabs/Tank 2 (Enemy)";
        var tank = Instantiate(Resources.Load(prefabName)) as GameObject;
        tank.name = name;
        tank.transform.position = new Vector3(position.x + 0.5f, position.y, position.z + 0.5f);
        tank.transform.SetParent(Scene.units.transform);

        // unit

        var u = tank.GetComponent<Unit>();
        if (isPlayerControlled)
        {
            instance.scene.player = u;
        }
        u.type = UNIT_TYPE.TANK;
        u.faction = faction;

        u.Destruction += Events.OnUnitDestruction;
        u.Destruction += OnUnitDestruction;
        u.EquipmentChange += Events.OnUnitEquipmentChange;
        u.HpChange += Events.OnUnitHealthChange;
        u.InteractionStart += Events.OnUnitInteractionStart;
        u.InteractionEnd += Events.OnUnitInteractionEnd;

        // equipment 

        var equipment = new List<Module> {
            instance.randomizer.Module.Hull.Default.Get(),
            instance.randomizer.Module.Turret.Default.Get(),
            instance.randomizer.Module.Chassis.Default.Get()
        };

        var gunType = instance.randomizer.WeaponType.Get();
        if (gunType == typeof(ArmorPenetratingCannon))
        {
            equipment.Add(instance.randomizer.Module.Weapon.AP.Get());
        }
        else
        {
            equipment.Add(instance.randomizer.Module.Weapon.DB.Get());
        }
        u.Mount(equipment);

        // behaviour

        
        if (isPlayerControlled)
        {
            tank.AddComponent<PlayerController>();
        }
        else
        {
            System.Type behaviour = botBehaviourType ?? typeof(Assaulter);
            if (botBehaviourType != null)
            {
                tank.AddComponent(botBehaviourType);
            }
            else
            {
                behaviour = instance.randomizer.Behaviour.Get();

                // checking not to create 2 supports

                var allies = GetAllies(faction);
                while (behaviour == typeof(Support) && allies.Any(x => x.GetType() == behaviour))
                {
                    behaviour = instance.randomizer.Behaviour.Get();
                }
            }
            tank.AddComponent(behaviour);
        }

        // assign to scene placeholders

        if (faction == UNIT_FACTION.PLAYER)
        {
            instance.scene.allies.Add(u);
            instance.alliesSpawned++;

        }
        else
        {
            instance.scene.enemies.Add(u);
            instance.enemiesSpawned++;
        }
        

        // event

        UnitSpawn?.Invoke(u);
    }

    public static void SpawnBase(string name, UNIT_FACTION faction, Vector3 position)
    {
        var prefab = GameObject.Instantiate(Resources.Load("Prefabs/Base")) as GameObject;
        prefab.transform.position = new Vector3(position.x, 0, position.z);
        prefab.transform.SetParent(Scene.buildings.transform);
        prefab.name = name;

        var u = prefab.GetComponent<Unit>();
        u.Destruction += OnUnitDestruction;
        u.HpChange += Events.OnBaseHealthChange;

        u.faction = faction;

        if (faction == UNIT_FACTION.PLAYER)
        {
            instance.scene.alliedBase = u;
        }
        else
        {
            instance.scene.enemyBase = u;
        }
    }

    public static void SpawnCrate(Vector3 position, Module item)
    {
        var crateObject = Instantiate(Resources.Load("Prefabs/Crate")) as GameObject;
        crateObject.transform.position = position;
        crateObject.name = "Crate";
        crateObject.transform.SetParent(Scene.interactive.transform);

        var crate = crateObject.GetComponent<Crate>();
        crate.module = item;
    }

    public static void OnUnitDestruction(Unit u)
    {
        if (u.type == UNIT_TYPE.BASE)
        {
            SetState(GAME_STATE.POST_GAME);
            if (u.faction == UNIT_FACTION.PLAYER)
            {
                SetState(GAME_STATE.GAME_LOST);
                Debug.Log("Player lost!");
            }
            else
            {
                SetState(GAME_STATE.GAME_WON);
                Debug.Log("Player won!");
            }
        }
        else
        {
            if (u == Scene.player)
            {
                SetState(GAME_STATE.GAME_LOST);
                Debug.Log("Player lost!");
            }
            else
            {
                GetAllies(u.faction).Remove(u);
                instance.ControlPopulation();
            }
        }
    }

    // methods

    public Game()
    {
        instance = this;
    }

    void Start()
    {
        events = GetComponent<EventController>();
        GameStateChange += Events.OnGameStateChange;
        UnitSpawn += Events.OnUnitSpawn;

        scene = GetComponent<Scene>();

        ui = scene.ui.GetComponent<UserInterfaceController>();
        ui.Initialize();

        SetState(GAME_STATE.MAIN_MENU);
    }

    public static void SetState(GAME_STATE state)
    {
        switch (state)
        {
            case GAME_STATE.MAIN_MENU:
                {
                    break;
                }
            case GAME_STATE.PRE_GAME:
                {
                    StartLevel();
                    return;
                }
            case GAME_STATE.IN_GAME:
                {

                    break;
                }
            case GAME_STATE.POST_GAME:
                {
                    break;
                }
            case GAME_STATE.QUIT:
                {
                    Application.Quit();
                    return;
                }
            default:
                {
                    break;
                }
        }

        State = state;
    }

    public static void StartLevel()
    {
        instance.CreateMap();

        // to check map generation in console
        /*

        LogMap();

        */

        instance.InstantiateMap();

        SpawnBase("Base (Player)", UNIT_FACTION.PLAYER, Map.bases[UNIT_FACTION.PLAYER]);
        SpawnBase("Base (Enemy)", UNIT_FACTION.ENEMY, Map.bases[UNIT_FACTION.ENEMY]);

        SpawnUnit("Tank (Player)", UNIT_FACTION.PLAYER, true);

        // no balance in specification, had to do this :c
        Scene.player.inputDamageMultiplier = 0.1f;

        // to manually spawn enemies
        /*
        SpawnUnit("Tank (Ally)", UNIT_FACTION.PLAYER, botBehaviourType: typeof(Support));

        SpawnUnit("Tank (Enemy)", UNIT_FACTION.ENEMY, botBehaviourType: typeof(Hunter));
        SpawnUnit("Tank (Enemy)", UNIT_FACTION.ENEMY, botBehaviourType: typeof(Support));
        */

        instance.ControlPopulation();

        State = GAME_STATE.PRE_GAME;

        SetState(GAME_STATE.IN_GAME);
    }

    public void CreateMap()
    {
        map = new NoiseMapGenerator();
        map.Generate(new Vector3(0, 0, 0), 40, 40);
    }

    public void InstantiateMap()
    {
        var floor = GameObject.Instantiate(Resources.Load("Prefabs/Floor")) as GameObject;
        floor.name = "Floor";
        floor.transform.position = Map.position;
        floor.transform.localScale = new Vector3(Map.width, Map.height, 1);
        floor.transform.SetParent(Scene.map.transform);

        for (int i = 0; i < Map.width; i++)
        {
            for (int j = 0; j < Map.height; j++)
            {
                var cell = Map.cells[i, j];
                switch (cell.type)
                {
                    case MAP_CELL_TYPE.WALL:
                        {
                            var prefab = GameObject.Instantiate(Resources.Load("Prefabs/Wall")) as GameObject;
                            prefab.transform.position = new Vector3(Map.position.x + i, 0, Map.position.z + j);
                            prefab.transform.parent = Scene.map.transform;
                            prefab.name = $"Wall ({i}, {j})";
                            break;
                        }
                    case MAP_CELL_TYPE.OBSTACLE:
                        {
                            var prefab = GameObject.Instantiate(Resources.Load("Prefabs/Obstacle")) as GameObject;
                            prefab.transform.position = new Vector3(Map.position.x + i, 0, Map.position.z + j);
                            prefab.transform.parent = Scene.map.transform;
                            prefab.name = $"Obstacle ({i}, {j})";
                            break;
                        }
                }
            }
        }
    }

    public void LogMap()
    {
        var s = "";
        for (int i = 0; i < Map.width; i++)
        {
            for (int j = 0; j < Map.height; j++)
            {
                s += Map.cells[i, j].type == MAP_CELL_TYPE.WALL ? "X" : "_";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    public void ControlPopulation()
    {
        var availableRespawns = maxArmySize - Scene.allies.Count;
        if (availableRespawns > 0)
        {
            for (int i = 0; i < availableRespawns; i++)
            {
                if (alliesSpawned < maxSpawnCount)
                {
                    SpawnUnit("Tank (Ally)", UNIT_FACTION.PLAYER);
                }
            }
        }

        availableRespawns = maxArmySize - Scene.enemies.Count;
        if (availableRespawns > 0)
        {
            for (int i = 0; i < availableRespawns; i++)
            {
                if (enemiesSpawned < maxSpawnCount)
                {
                    SpawnUnit("Tank (Enemy)", UNIT_FACTION.ENEMY);
                }
            }
        }
    }

}
