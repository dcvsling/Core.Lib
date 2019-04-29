using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.Lib.Ast.Models
{

    [DebuggerDisplay("{Name}:{Value}:{Range}")]
    public struct Token : IEqualityComparer<Token>
    {
        private static ConcurrentDictionary<string, Token> _tokens = new ConcurrentDictionary<string, Token>();

        public string Name;
        public string Value;
        public Range Range;
        private static Token _default = default;

        public override bool Equals(object obj) => obj.GetHashCode() == GetHashCode();
        public override int GetHashCode() => GetHashCode(this);

        public bool Equals(Token x, Token y) => GetHashCode(x) == GetHashCode(y);
        public int GetHashCode(Token obj) => (obj.Value?.GetHashCode() << 2) ^ obj.Name?.GetHashCode() ?? 0;

        public static implicit operator int(Token token) => token.Value.GetHashCode();
        public static implicit operator string(Token token) => token.Value;

        public static implicit operator Token(string str)
            => _tokens.GetOrAdd(str, new Token { Name = str, Value = str });

        public static implicit operator Token((string name, string val) token)
            => _tokens.GetOrAdd(token.val, new Token { Name = token.name, Value = token.val });

        public static implicit operator Token((Token val, Range range) token)
            => new Token { Name = token.val.Name, Value = token.val.Value, Range = token.range };

        public static bool operator ==(Token x, Token y)
            => x.Equals(x, y);
        public static bool operator !=(Token x, Token y)
            => !(x == y);
    }
}