using System;
using System.Collections.Generic;
using System.Linq;
using game.GameEngine.Components;
using rpg_v2;

namespace game.GameEngine
{
    public struct Node
    {
        private int _x;
        private int _y;

        public int X
        {
            readonly get => _x;
            set => _x = value;
        }

        public int Y
        {
            readonly get => _y;
            set => _y = value;
        }

        public Node(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node other)
                return other._x == this._x && other._y == this._y;

            return false;
        }

        public bool Equals(Node other)
        {
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }

        public List<Node> FindNeighbours(HashSet<(int, int)> positions)
        {
            List<Node> neighbours = new List<Node>();
            
            var pX = X;
            var pY = Y;
            
            if(positions.TryGetValue((pX + 1, pY), out _))
                neighbours.Add(new Node(pX + 1, pY));
            
            if (positions.TryGetValue((pX - 1, pY), out _))
                neighbours.Add(new Node(pX - 1, pY));
            
            if (positions.TryGetValue((pX, pY + 1), out _))
                neighbours.Add(new Node(pX, pY + 1));
            
            if (positions.TryGetValue((pX, pY - 1), out _))
                neighbours.Add(new Node(pX, pY - 1));
            
            return neighbours;
        }
    }
}