namespace Core.Lib.Ast.Lexer
{
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.Extensions.Options;
    using Models;
    using Reflections;
    using System;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    internal sealed class ActionStore : IActionStore
    {
        private readonly IOptionsMonitor<AstOptions> _monitor;
        private AstOptions _options;
        private readonly IOptionsMonitorCache<Task<Func<char, bool>>> _cache;
        private readonly IDisposable _disposer;
        public ActionStore(IOptionsMonitor<AstOptions> monitor, IOptionsMonitorCache<Task<Func<char, bool>>> cache)
        {
            _monitor = monitor;
            _cache = cache;
            _disposer = _monitor.OnChange(o => _options = o);
            _options = _monitor.CurrentValue;
        }

        public Task<Func<char, bool>> Get(Behavior behavior)
            => _cache.GetOrAdd(behavior.Action, () => Create(behavior));

        private Task<Func<char, bool>> Create(Behavior behavior)
        {
            var exps = _options.Behaviors.ContainsKey(behavior.Action)
                ? _options.Behaviors[behavior.Action]
                    .Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)
                : throw new ArgumentException($"no such action {behavior.Action} exists");

            var exp = exps.Last();
            var parameters = exps.Except(exp.Emit())
                .SelectMany(x => x.Split(','))
                .Select(x => x.Trim().IndexOf(" ") > 0 ? "object " + x : x);


            var invoker = CSharpScript.Create(exp, globalsType: typeof(ExpandoObject));
            invoker.Compile();
            //var exec = MethodExecutor.CreateExecutor((invoker.RunFromA).Method);
            return default;//c => (bool)exec.Execute(invoker, behavior.Value, c);
        }

        public void Dispose()
        {
            _disposer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}