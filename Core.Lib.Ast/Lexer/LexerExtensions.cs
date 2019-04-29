using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Lexer;
using Core.Lib.Ast.Lexer.Operations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LexerExtensions
    {
        public static AstBuilder AddLexer(this AstBuilder builder)
        {
            builder.Services.AddSingleton<ILexer, Lexer>()
                .AddSingleton<ILexerOperationFactory, LexerOperationFactory>();
            return builder;
        }

        public static AstBuilder AddDefaultLexer(this AstBuilder builder)
        {
            builder.AddLexer()
                .Services
                .AddSingleton<ILexerOperation, StringLexerOperation>()
                .AddSingleton<ILexerOperation, NumberLexerOperation>()
                .AddSingleton<ILexerOperation, SignLexerOperation>();
            return builder;
        }
    }
}
