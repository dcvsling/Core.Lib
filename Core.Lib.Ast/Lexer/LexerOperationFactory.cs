namespace Core.Lib.Ast.Lexer
{
    using Abstractions;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;

    internal class LexerOperationFactory : ILexerOperationFactory
    {
        private readonly IOptionsMonitorCache<ILexerOperation> _cache;
        private readonly IEnumerable<ILexerOperation> _operators;

        public LexerOperationFactory(
            IEnumerable<ILexerOperation> operators,
            IOptionsMonitorCache<ILexerOperation> cache)
        {
            _cache = cache;
            _operators = operators;
        }


        public ILexerOperation GetLexerOperation(string name)
            => _cache.GetOrAdd(name, () => CreateLexerOperation(name));

        private ILexerOperation CreateLexerOperation(string name)
           => _operators.Aggregate(
               LexerOperation.Root,
               (root, op) => root.Append(op));
    }
}