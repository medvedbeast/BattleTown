using Enumerations;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class Map
{
    public Cell[,] cells { get; set; }
    public Vector3 position;
    public int width;
    public int height;

    public Dictionary<UNIT_FACTION, Vector3> bases = new Dictionary<UNIT_FACTION, Vector3>();
    public Dictionary<UNIT_FACTION, List<Vector3>> spawnPoints = new Dictionary<UNIT_FACTION, List<Vector3>>
    {
        [UNIT_FACTION.PLAYER] = new List<Vector3>(),
        [UNIT_FACTION.ENEMY] = new List<Vector3>()
    };

    public virtual void Generate(Vector3 position, int width, int height) { }

    public void InitializeMap(Vector3 position, int width, int height)
    {
        this.position = position;
        this.width = width;
        this.height = height;

        cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                MAP_CELL_TYPE type = MAP_CELL_TYPE.OBSTACLE;
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    type = MAP_CELL_TYPE.WALL;
                }
                cells[i, j] = new Cell { type = type };
            }
        }
    }

    public Vector2 RandomizeBasePosition(int roadSize, int baseSize, UNIT_FACTION faction)
    {
        if (faction == UNIT_FACTION.PLAYER)
        {
            return new Vector2(1, Random.Range(1, height - (roadSize * 2 + baseSize)));
        }
        else
        {
            return new Vector2(width - (roadSize * 2 + baseSize) - 1, Random.Range(1, height - (roadSize * 2 + baseSize)));
        }
    }

    public void InsertBase(Vector2 position, int roadSize, int baseSize, UNIT_FACTION faction)
    {
        bases.Add(faction, new Vector3(position.x + roadSize, 0, position.y + roadSize));

        for (int i = 0; i < roadSize * 2 + baseSize; i++)
        {
            for (int j = 0; j < roadSize * 2 + baseSize; j++)
            {
                var type = MAP_CELL_TYPE.ROAD;
                if ((i >= roadSize && i < roadSize + baseSize) && (j >= roadSize && j < roadSize + baseSize))
                {
                    type = MAP_CELL_TYPE.BUILDING;
                }
                else
                {
                    spawnPoints[faction].Add(new Vector3(position.x + i, 0.0f, position.y + j));
                }
                cells[(int)position.x + i, (int)position.y + j].type = type;
            }
        }
    }
}
