using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Lexer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LexerExtensions
    {
        public static AstBuilder AddLexer(this AstBuilder builder)
        {
            builder.Services.AddSingleton<ILexer, Lexer>()
                .AddSingleton<ILexerOperationFactory, LexerOperationFactory>()
                .AddSingleton<IActionStore, ActionStore>();
            return builder;
        }
    }
}
