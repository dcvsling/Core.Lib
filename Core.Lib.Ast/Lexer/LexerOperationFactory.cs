namespace Core.Lib.Ast.Lexer
{
    using Abstractions;
    using Microsoft.Extensions.Options;
    using Models;
    using System.Linq;

    internal class LexerOperationFactory : ILexerOperationFactory
    {
        private readonly IOptionsSnapshot<AstOptions> _snapshot;
        private readonly IActionStore _store;
        private readonly IOptionsMonitorCache<ILexerOperation> _cache;

        public LexerOperationFactory(
            IOptionsSnapshot<AstOptions> snapshot,
            IActionStore store,
            IOptionsMonitorCache<ILexerOperation> cache)
        {
            _snapshot = snapshot;
            _store = store;
            _cache = cache;
        }


        public ILexerOperation GetLexerOperation(string name)
            => _cache.GetOrAdd(name, () => CreateLexerOperation(name));

        private ILexerOperation CreateLexerOperation(string name)
           => _snapshot.Get(name).Operations.Aggregate(
               LexerOperation.Root,
               (root, op) => root.Append(new OperatorLexerOperation(op, _store)));
    }
}