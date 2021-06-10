using Enumerations;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator : Map
{
    public override void Generate(Vector3 position, int width, int height)
    {
        InitializeMap(position, width, height);

        int roadSize = 2;
        int baseSize = 2;

        float noiseX = Random.Range(0, 1000);
        float noiseY = Random.Range(0, 1000);
        float noiseScale = 20.0f;

        float threshold = 0.25f;

        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                var value = Mathf.PerlinNoise(noiseX + (float)i / width * noiseScale, noiseY + (float)j / width * noiseScale);
                if (value > threshold)
                {
                    cells[i, j].type = MAP_CELL_TYPE.ROAD;
                }
            }
        }

        var b1 = RandomizeBasePosition(roadSize, baseSize, UNIT_FACTION.PLAYER);
        InsertBase(b1, roadSize, baseSize, UNIT_FACTION.PLAYER);

        var b2 = RandomizeBasePosition(roadSize, baseSize, UNIT_FACTION.ENEMY);
        InsertBase(b2, roadSize, baseSize, UNIT_FACTION.ENEMY);

        var path = Game.Pathfinder.Search(this, b1, b2);
        if (path == null)
        {
            Game.Pathfinder.CreateShortestPath(this, b1, b2);
        }
    }
}
