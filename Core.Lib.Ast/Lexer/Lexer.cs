using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Lexer
{
    internal class Lexer : ILexer
    {
        private readonly ILexerOperation _factory;

        public Lexer(ILexerOperation factory)
        {
            _factory = factory;
        }
        public Task<IEnumerable<Token>> Lex(ReadOnlyMemory<char> source)
            => new LexerContext(_factory).GetTokens(source);
    }
}