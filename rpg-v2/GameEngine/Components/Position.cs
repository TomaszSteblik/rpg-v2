using System;
using System.Text.Json.Serialization;

namespace game.GameEngine.Components
{
    public class Position : Component
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
            X = 1;
            Y = 1;
        }

        #nullable enable
        public override bool Equals(object? obj)
        {
            if (obj is not Position otherPosition)
                return false;

            return otherPosition.X == this.X && otherPosition.Y == this.Y;

        }

        protected bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}