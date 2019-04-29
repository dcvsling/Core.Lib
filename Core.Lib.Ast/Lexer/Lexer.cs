using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Core.Lib.Ast.Lexer
{
    internal class Lexer : ILexer
    {
        private readonly ILexerOperationFactory _factory;
        private readonly ILoggerFactory _logger;

        public Lexer(ILexerOperationFactory factory, ILoggerFactory logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public IEnumerable<Token> Lex(string source)
        {
            var ctx = new LexerContext(_factory.GetLexerOperation(string.Empty), _logger.CreateLogger<LexerContext>());
            return ctx.GetTokens(source);
        }
    }
}