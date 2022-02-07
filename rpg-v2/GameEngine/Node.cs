using System;
using System.Collections.Generic;
using System.Linq;
using game.GameEngine.Components;
using rpg_v2;

namespace game.GameEngine
{
    public struct Node
    {
        public int X { get; set; }
        public int Y{ get; set; }

        public Node(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
            
        public override bool Equals(object obj)
        {
            if (obj is Node other)
                return other.X == this.X && other.Y == this.Y;

            return false;
        }

        public bool Equals(Node other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public List<Node> FindNeighbours()
        {
            List<Node> neighbours = new List<Node>();



            var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                .Where(x=>((Physics) x.Components[3]).IsCollidable).ToArray();
            
            
            var pX = X;
            var pY = Y;
            
            
            
            if(!collidables.Any(x=>((Position) x.Components[0]).X == pX+1 && ((Position) x.Components[0]).Y == pY))
                //if(SourceOfMagic.Map.CanMoveInto(X+1,Y))
                neighbours.Add(new Node(X+1,Y));
                
            if(!collidables.Any(x=>((Position) x.Components[0]).X == pX-1 && ((Position) x.Components[0]).Y == pY))
                //if(SourceOfMagic.Map.CanMoveInto(X-1,Y))
                neighbours.Add(new Node(X-1,Y));
                
            if(!collidables.Any(x=>((Position) x.Components[0]).X == pX && ((Position) x.Components[0]).Y == pY+1))
                //if(SourceOfMagic.Map.CanMoveInto(X,Y+1))
                neighbours.Add(new Node(X,Y+1));
                
            if(!collidables.Any(x=>((Position) x.Components[0]).X == pX && ((Position) x.Components[0]).Y == pY-1))
                //if(SourceOfMagic.Map.CanMoveInto(X,Y-1))
                neighbours.Add(new Node(X,Y-1));
                
                
            return neighbours;
        }
    }
}