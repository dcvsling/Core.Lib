namespace Core.Lib.Ast.Lexer
{
    using Core.Lib.Ast.Abstractions;
    using Microsoft.Extensions.Logging;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class LexerContext : IDisposable
    {
        private readonly ILexerOperation _operations;
        private readonly ILogger<LexerContext> _logger;

        public LexerContext(ILexerOperation operations, ILogger<LexerContext> logger)
        {
            _operations = operations;
            _logger = logger;
        }

        public void Dispose()
        {
        }
        public IEnumerable<Token> GetTokens(string span)
        {
            var location = new Location();
            LexerOperationContext ctx = default;
            _logger.OnLexerStart();

            while (span.Any())
            {
                try
                {
                    ctx = new LexerOperationContext { Content = span };
                    _operations.GetToken(ctx);
                }
                catch (Exception ex)
                {

                    throw new LexerOperationException(ctx, location, ex.TargetSite.DeclaringType, ex);
                }
                if (!ctx.IsSuccess)
                    break;
                var newtoken = ctx.Token;
                var newloca = location;
                if (newtoken.Name == nameof(Environment.NewLine))
                {
                    newloca.Column = 0;
                    newloca.Line += 1;
                }
                else
                {
                    newloca.Column += newtoken.Value.Length;
                }
                newloca.Index += newtoken.Value.Length;
                yield return (newtoken, (location, newloca));
                location = newloca;
                span = span.Substring(newtoken.Value.Length);
            }
            _logger.OnLexerEnd();
        }
    }
}