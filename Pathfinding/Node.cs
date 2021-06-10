using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public Vector2 position;
        public int pathLengthFromStart;
        public int pathLengthToEnd;
        public Node previousNode;

        public int EstimatedPathLength => pathLengthFromStart + pathLengthToEnd;
    }
}