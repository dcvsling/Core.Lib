using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using Microsoft.Extensions.Logging;

namespace Core.Lib.Ast.Lexer.Operations
{
    internal class StringLexerOperation : ILexerOperation
    {
        private const string Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
        private readonly ILogger<StringLexerOperation> _logger;

        public StringLexerOperation(ILogger<StringLexerOperation> logger)
        {
            _logger = logger;
        }
        public void GetToken(LexerOperationContext context)
        {
            if (!IsWord(context.Content[0])) return;
            CreateToken(context);
        }

        private bool IsWord(char c) => Text.IndexOf(c) >= 0;
        private void CreateToken(LexerOperationContext context)
        {
            _logger.OnOperationMatching(context, typeof(StringLexerOperation));
            var count = 1;
            while (context.Content.Length > count && IsWord(context.Content[count])) { count++; };
            context.Token = new Token {
                Name = nameof(Text),
                Value = context.Content.Substring(0, count)
            };
            context.IsSuccess = true;
            _logger.OnTokenCreatedOperation(context, typeof(StringLexerOperation));
        }
    }
}