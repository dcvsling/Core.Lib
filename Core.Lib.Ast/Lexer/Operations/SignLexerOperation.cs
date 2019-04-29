using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Internal;
using Core.Lib.Ast.Models;
using Microsoft.Extensions.Logging;

namespace Core.Lib.Ast.Lexer.Operations
{

    internal class SignLexerOperation : ILexerOperation
    {
        private readonly Operators _operators;

        public SignLexerOperation(ILogger<SignLexerOperation> logger)
        {
            _logger = logger;
            _operators = Operators.LoadDefualt(_logger);
        }

        public void GetToken(LexerOperationContext context)
        {
            if (!IsSign(context.Content[0])) return;
            CreateToken(context);
        }

        private bool IsSign(char c) => WORD.IndexOf(c) < 0;

        private void CreateToken(LexerOperationContext context)
        {
            _logger.OnOperationMatching(context, typeof(SignLexerOperation));
            var count = 1;
            while (context.Content.Length > count && IsSign(context.Content[count]) && count < 4) { count++; }

            while (!_operators.ContainsKey(context.Content.Substring(0, count)) && count != 0) { count--; }
            var value = context.Content.Substring(0, count == 0 ? 1 : count);
            context.Token = new Token {
                Name = $"{(count != 0 ? _operators[value].Name : context.Content[0].ToString())}",
                Value = value
            };
            context.IsSuccess = true;
            _logger.OnTokenCreatedOperation(context, typeof(SignLexerOperation));
        }



        private const string WORD = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,_";
        private readonly ILogger<SignLexerOperation> _logger;
    }
}