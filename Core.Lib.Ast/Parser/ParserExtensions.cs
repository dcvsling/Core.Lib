using Core.Lib.Ast.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ParserExtensions
    {
        public static AstBuilder AddParser(this AstBuilder builder)
            => builder;
    }
}
