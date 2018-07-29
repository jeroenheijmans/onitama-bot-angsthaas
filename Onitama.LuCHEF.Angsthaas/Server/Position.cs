namespace Onitama.LuCHEF.Angsthaas.Server
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsOutOfBounds => X < 0 || X > 4 || Y < 0 || Y > 4;

        public override bool Equals(object obj) => obj is Position && Equals((Position)obj);
        public bool Equals(Position other) => this == other;
        public static bool operator ==(Position left, Position right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Position left, Position right) => !(left == right);

        public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y);

        public override int GetHashCode()
        {
            // Based on https://github.com/dotnet/corefx/blob/84f5ac7fc30a7373e4ecdaabec0bf1bd653f8279/src/Common/src/System/Numerics/Hashing/HashHelpers.cs

            unchecked
            {
                uint rol5 = ((uint)X << 5) | ((uint)Y >> 27);
                return ((int)rol5 + X) ^ Y;
            }
        }

        public static Position Player1Home => new Position(0, 2);
        public static Position Player2Home => new Position(4, 2);
    }
}
