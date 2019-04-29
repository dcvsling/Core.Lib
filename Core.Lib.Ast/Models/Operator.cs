namespace Core.Lib.Ast.Models
{
    using Internal;
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Name}({Value})")]
    public struct Operator
    {
        public string Name;
        public string Value;
        public Lazy<VisitMiddleware> Middleware;
        public override bool Equals(object obj) => GetHashCode().Equals(obj?.GetHashCode());
        public override int GetHashCode() => (Name?.GetHashCode() >> 2) ^ Value?.GetHashCode() ?? 0;
    }
}