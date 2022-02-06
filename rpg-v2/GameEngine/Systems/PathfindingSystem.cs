using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace game.GameEngine.Systems
{
    public static class PathfindingSystem
    {
        public static void Act()
        {
            
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