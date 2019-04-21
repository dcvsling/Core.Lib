namespace Microsoft.Extensions.DependencyInjection
{
    using Core.Lib.Ast.Abstractions;
    public static class AstExtensions
    {
        public static AstBuilder AddAst(this IServiceCollection services)
           => new AstBuilder(services).AddCoreService();
    }

}