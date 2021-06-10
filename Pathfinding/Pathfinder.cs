using Enumerations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder
    {
        public static Pathfinder Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Pathfinder();
                }
                return _instance;
            }
        }

        private static Pathfinder _instance;

        //

        public int Distance(Vector2 a, Vector2 b)
        {
            return Mathf.RoundToInt(Vector2.Distance(a, b));
        }

        public List<Vector2> Search(Map map, Vector2 from, Vector2 to)
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node
            {
                position = from,
                previousNode = null,
                pathLengthFromStart = 0,
                pathLengthToEnd = Distance(from, to)
            });

            while (open.Count > 0)
            {
                Node current = open.OrderBy(x => x.EstimatedPathLength).First();

                if (current.position == to)
                {
                    return GetPathToNode(current);
                }

                open.Remove(current);
                closed.Add(current);


                var neighbours = GetNeighbours(map, current, to);
                foreach (var n in neighbours)
                {
                    if (closed.Any(x => x.position == n.position))
                    {
                        continue;
                    }

                    var next = open.FirstOrDefault(x => x.position == n.position);

                    if (next == null)
                    {
                        open.Add(n);
                    }
                    else if (next.pathLengthFromStart > n.pathLengthFromStart)
                    {
                        next.previousNode = current;
                        next.pathLengthFromStart = n.pathLengthFromStart;
                    }
                }
            }
            return null;
        }

        public List<Node> GetNeighbours(Map map, Node node, Vector2 to, bool searchCorners = false)
        {
            /*

                x 0 x
                3 x 1
                x 2 x

            */

            var result = new List<Node>();

            var validNeighbours = new bool[]
            {
                false,
                false,
                false,
                false
            };

            var neighbours = new Vector2[]
            {
                new Vector2(node.position.x, node.position.y + 1),
                new Vector2(node.position.x + 1, node.position.y),
                new Vector2(node.position.x, node.position.y - 1),
                new Vector2(node.position.x - 1, node.position.y)
            };

            int i = 0;
            foreach (var p in neighbours)
            {
                if (p.x < 0 || p.x >= map.width || p.y < 0 || p.y >= map.height)
                {
                    continue;
                }

                if (map.cells[(int)p.x, (int)p.y].type != MAP_CELL_TYPE.ROAD)
                {
                    continue;
                }

                result.Add(new Node()
                {
                    position = p,
                    previousNode = node,
                    pathLengthFromStart = node.pathLengthFromStart + 1,
                    pathLengthToEnd = Distance(p, to)
                });
                validNeighbours[i] = true;

                i++;
            }

            if (searchCorners)
            {
                var corners = new Vector2[]
                {
                    new Vector2(node.position.x + 1, node.position.y + 1),
                    new Vector2(node.position.x + 1, node.position.y - 1),
                    new Vector2(node.position.x - 1, node.position.y - 1),
                    new Vector2(node.position.x - 1, node.position.y + 1)
                };


                i = 0;
                foreach (var p in corners)
                {
                    if (p.x < 0 || p.x >= map.width || p.y < 0 || p.y >= map.height)
                    {
                        continue;
                    }

                    if (map.cells[(int)p.x, (int)p.y].type != MAP_CELL_TYPE.ROAD)
                    {
                        continue;
                    }

                    if (validNeighbours[i] && validNeighbours[i + 1 < validNeighbours.Length ? i + 1 : 0])
                    {
                        result.Add(new Node()
                        {
                            position = p,
                            previousNode = node,
                            pathLengthFromStart = node.pathLengthFromStart + 1,
                            pathLengthToEnd = Distance(p, to)
                        });
                    }

                    i++;
                }
            }

            return result;
        }

        public void CreateShortestPath(Map map, Vector2 from, Vector2 to)
        {
            var current = from;

            while (current != to)
            {
                map.cells[(int)current.x, (int)current.y].type = MAP_CELL_TYPE.ROAD;

                if (current.x != to.x)
                {
                    if (current.x < to.x)
                    {
                        current.x++;
                    }
                    else
                    {
                        current.x--;
                    }
                }
                else if (current.y != to.y)
                {
                    if (current.y < to.y)
                    {
                        current.y++;
                    }
                    else
                    {
                        current.y--;
                    }
                }
            }
        }

        public List<Vector2> GetPathToNode(Node node)
        {
            var result = new List<Vector2>();
            var currentNode = node;
            while (currentNode != null)
            {
                result.Add(currentNode.position);
                currentNode = currentNode.previousNode;
            }
            result.Reverse();
            return result;
        }

        public Vector3? GetClosestEmptyPoint(Map map, Vector3 target)
        {
            var point = new Vector2(Mathf.Round(target.x), Mathf.Round(target.z));

            if (InBounds(map, point))
            {
                if (map.cells[(int)point.x, (int)point.y].type == MAP_CELL_TYPE.ROAD)
                {
                    return new Vector3(point.x, target.y, point.y);
                }

                var offset = 0;
                var maxOffset = 2;
                do
                {
                    var x = point.x - offset;
                    var y = point.y - offset;
                    for (int i = 0; i < offset * 2 + 1; i++)
                    {
                        for (int j = 0; j < offset * 2 + 1; j++)
                        {
                            if (InBounds(map, new Vector2(x, y)))
                            {
                                if (map.cells[(int)x, (int)y].type == MAP_CELL_TYPE.ROAD)
                                {
                                    return new Vector3(x, 0, y);
                                }
                            }
                        }
                    }
                    offset++;
                }
                while (offset <= maxOffset);

                return null;
            }

            return null;
        }

        public Vector3 GetEmptySpawnPoint(Map map, UNIT_FACTION faction)
        {
            var points = map.spawnPoints[faction];
            var units = faction == UNIT_FACTION.PLAYER ? Game.Scene.allies : Game.Scene.enemies;
            var empty = new List<Vector3>(points);
            foreach (var u in units)
            {
                foreach (var p in points)
                {
                    if (Vector3.Distance(u.transform.position, p) <= 1.5f)
                    {
                        empty.Remove(p);
                    }
                }

            }
            return empty.FirstOrDefault();
        }

        public List<Vector3> CellsInRadius(Map map, Vector3 point, int radius, MAP_CELL_TYPE type)
        {
            var result = new List<Vector3>();

            var start = new Vector3Int(Mathf.CeilToInt(point.x - radius), 0, Mathf.CeilToInt(point.z - radius));
            for (int i = 0; i < radius * 2; i++)
            {
                for (int j = 0; j < radius * 2; j++)
                {
                    var currentPoint = new Vector3Int(start.x + i, 0, start.z + j);
                    if (Vector3.Distance(currentPoint, point) <= radius && InBounds(map, new Vector2(currentPoint.x, currentPoint.z)))
                    {
                        if (map.cells[currentPoint.x, currentPoint.z].type == type)
                        {
                            result.Add(new Vector3(currentPoint.x, currentPoint.y, currentPoint.z));
                        }
                    }
                }
            }

            return result;
        }

        public bool InBounds(Map map, Vector2 point)
        {
            if (point.x < 0 || point.x >= map.width || point.y < 0 || point.y >= map.height)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}

