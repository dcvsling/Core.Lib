using System.Diagnostics;

namespace Core.Lib.Ast.Models
{
    [DebuggerDisplay("{DebugView}")]
    public struct Location
    {
        public int Column;

        public int Line;

        public int Index;
        internal string DebugView => ToString();
        public override string ToString() => $"({Line}:{Column}:{Index})";

        public override int GetHashCode() => ((Column.GetHashCode() << 1) ^ Line.GetHashCode() << 1) ^ Index.GetHashCode();
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public static implicit operator int(Location location) => location.GetHashCode();
        public static implicit operator string(Location location) => location.ToString();

        public static implicit operator Location((int line, int column) location)
            => new Location { Column = location.line, Line = location.column };

        public static bool operator ==(Location x, Location y)
            => x.Equals(y);
        public static bool operator !=(Location x, Location y)
            => !(x == y);
    }

}