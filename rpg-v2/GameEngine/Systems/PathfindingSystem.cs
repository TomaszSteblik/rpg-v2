using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using game.GameEngine.Components;

namespace game.GameEngine.Systems
{
    public static class PathfindingSystem
    {
        public static void Act()
        {
            var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 0, 5 });
            foreach (var entity in entities)
            {
                var pathfindingComponent = (Pathfinding)entity.Components[5];

                if (pathfindingComponent.NeedToFindNewPath is false)
                    continue;

                var position = (Position)entity.Components[0];

                var didFindPath = FindPath(position.X, position.Y,
                    pathfindingComponent.TargetX, pathfindingComponent.TargetY,
                    out var foudPath);

                if (didFindPath is false)
                {
                    Debug.WriteLine($"Failed to find path to X: {pathfindingComponent.TargetX} Y: {pathfindingComponent.TargetY}");
                    continue;
                }
                Debug.WriteLine("Succeded to find path");
                pathfindingComponent.Path = foudPath;
                pathfindingComponent.Step = 0;
                pathfindingComponent.NeedToFindNewPath = false;

            }
        }

        [Description("Returns distance between two nodes, using pitagoras theorem")]
        public static double CalculateDistance(Node start, Node goal)
        {
            return Math.Pow((start.X - goal.Y), 2) + Math.Pow(start.Y - goal.Y, 2);
        }

        [Description("Returns true if can return out var list of nodes(path) from starting node to destination node, " +
                     "returns false if can't reach destination")]
        public static bool FindPath(int startX, int startY, int destX, int destY, out List<Node> path)
        {
            var empty = new Node(-1, -1);
            var goal = new Node(destX, destY);
            var frontier = new Queue<Node>();
            var cameFrom = new Dictionary<Node, Node>();
            var start = new Node(startX, startY);
            cameFrom.Add(start, empty);
            frontier.Enqueue(start);

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(goal))
                    break;
                var nexts = current.FindNeighbours();
                for (int i = 0; i < nexts.Count; i++)
                {
                    if (!cameFrom.ContainsKey(nexts[i]))
                    {
                        frontier.Enqueue(nexts[i]);
                        cameFrom.Add(nexts[i], current);
                    }
                }
            }

            path = new List<Node>();
            {
                var current = goal;
                if (!cameFrom.ContainsKey(current))
                {
                    //nie da sie dotrzec do wyznaczonego punktu
                    return false;
                }

                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
            }
            path.Reverse();
            return true;
        }

    }
}