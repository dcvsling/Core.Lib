using System.Linq;
using Core.Lib.Ast.Models;
using Core.Lib.Reflections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{

    internal class AstOptionsConfigureOptions : IConfigureNamedOptions<AstOptions>
    {
        private readonly IConfiguration _config;

        public AstOptionsConfigureOptions(IConfiguration config)
        {
            _config = config;
        }
        public void Configure(string name, AstOptions options)
        {
            _config.GetSection("var").GetChildren()
                .Where(x => !options.Variables.Keys.Contains(x.Key))
                .Each(x => options.Variables.Add(x.Key, x.Value));

            var actions = _config.GetSection("action");
            if (actions.Exists())
                _config.GetSection("action").GetChildren()
                    .Where(x => !options.Behaviors.Keys.Contains(x.Key))
                    .Each(x => options.Behaviors.Add(x.Key, x.Value));

            _config.GetSection("operations").Bind(options.Operations);

            var rules = _config.GetSection("rules");
            if (rules.Exists())
                rules.GetChildren()
                    .Where(x => !options.Behaviors.Keys.Contains(x.Key))
                    .Each(x => options.Behaviors.Add(x.Key, x.Value));
        }
        public void Configure(AstOptions options) => Configure(string.Empty, options);
    }
}
