using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The named path configure options class.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    internal class NamedPathConfigureOptions<TOptions> : IConfigureNamedOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// The config (readonly).
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPathConfigureOptions{TOptions}"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public NamedPathConfigureOptions(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="options">The options.</param>
        public void Configure(string name, TOptions options)
            => _config.GetSection(name).Bind(options);

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Configure(TOptions options)
            => Configure(string.Empty, options);
    }
}
