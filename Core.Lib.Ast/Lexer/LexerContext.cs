namespace Core.Lib.Ast.Lexer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Lib.Ast.Abstractions;
    using Models;

    public sealed class LexerContext : IDisposable
    {

        internal static LexerContext Default
            => new LexerContext(new BasicLexerOperation());
        private readonly ILexerOperation _operations;
        public Token CurrentToken { get; private set; }
        public LexerContext(ILexerOperation operations)
        {
            _operations = operations;
        }

        public void Dispose()
        {
        }
        async public Task<IEnumerable<Token>> GetTokens(ReadOnlyMemory<char> span)
        {
            var location = new Location();
            var tokens = new List<Token>();
            while (span.Length > 0)
            {
                var newtoken = await _operations.GetTokenAsync(span);
                var newloca = location;
                if (newtoken.Value == "\n")
                {
                    newloca.Column = 0;
                    newloca.Line += 1;
                }
                else
                {
                    newloca.Column += newtoken.Value.Length;
                }
                CurrentToken = (newtoken, (location, newloca));
                location = newloca;
                tokens.Add(CurrentToken);
                span = span.Slice(CurrentToken.Value.Length);

            }
            return tokens;
        }
    }
}