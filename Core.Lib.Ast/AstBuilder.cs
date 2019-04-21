using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Core.Lib.Tests")]

namespace Core.Lib.Ast.Abstractions
{
    using Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Linq;

    public class AstBuilder
    {
        public AstBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal AstBuilder AddCoreService()
        {
            if (Services.Any(x => x.ServiceType == typeof(AstBuilder))) return this;

            Services.AddOptions()
                .AddSingleton(typeof(IConfigureOptions<>), typeof(NamedPathConfigureOptions<>))
                .AddScoped<IVariableTokenFormatterFactory, VariableTokenFormatterFactory>();

            this.AddLoader()
                .AddLexer();


            Services.AddSingleton(this);
            return this;
        }

        public IServiceCollection Services { get; }
    }

}