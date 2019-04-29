using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using Microsoft.Extensions.Logging;

namespace Core.Lib.Ast.Lexer.Operations
{
    internal class NumberLexerOperation : ILexerOperation
    {
        private const string Number = "0123456789,_";
        private readonly ILogger<NumberLexerOperation> _logger;

        public NumberLexerOperation(ILogger<NumberLexerOperation> logger)
        {
            _logger = logger;
        }
        public void GetToken(LexerOperationContext context)
        {
            if (!IsNumber(context.Content[0])) return;
            CreateToken(context);
        }

        private bool IsNumber(char c) => Number.IndexOf(c) >= 0;
        private void CreateToken(LexerOperationContext context)
        {
            _logger.OnOperationMatching(context, typeof(NumberLexerOperation));
            var count = 1;
            while (context.Content.Length > count && IsNumber(context.Content[count])) { count++; };
            context.Token = new Token {
                Name = nameof(Number),
                Value = context.Content.Substring(0, count)
            };
            context.IsSuccess = true;
            _logger.OnTokenCreatedOperation(context, typeof(NumberLexerOperation));
        }
    }
}