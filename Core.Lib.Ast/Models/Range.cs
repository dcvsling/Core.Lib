using System.Diagnostics;

namespace Core.Lib.Ast.Models
{
    [DebuggerDisplay("{Start}-{End}")]
    public struct Range
    {
        public Location Start;
        public Location End;
        public override string ToString() => $"{{{Start.Index}-{End.Index}}}";

        public static implicit operator string(Range range)
            => range.ToString();

        public static implicit operator Range((Location start, Location end) range)
            => new Range { Start = range.start, End = range.end };
    }

}