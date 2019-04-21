namespace Core.Lib.Ast.Lexer
{
    using System;
    using System.Threading.Tasks;
    using Abstractions;
    using Models;
    internal class LexerOperation : ILexerOperation, IChainableOperation
    {
        private ILexerOperation _next;
        private readonly ILexerOperation _last;

        public static IChainableOperation Root => new EmptyOperation();

        internal LexerOperation(ILexerOperation last)
        {
            _last = last;
        }

        async public virtual Task<Token> GetTokenAsync(ReadOnlyMemory<char> span)
            => (await _last.GetTokenAsync(span)) is Token token != default
                ? token
                : await _next.GetTokenAsync(span);

        IChainableOperation IChainableOperation.Append(ILexerOperation operation)
        {
            _next = operation;
            return new LexerOperation(this);
        }

        private class EmptyOperation : ILexerOperation, IChainableOperation
        {
            private ILexerOperation _operation = default;
            IChainableOperation IChainableOperation.Append(ILexerOperation operation)
            {
                _operation = operation;
                return new LexerOperation(this);
            }
            async public Task<Token> GetTokenAsync(ReadOnlyMemory<char> memory)
                => (await _operation.GetTokenAsync(memory)) is Token token != default
                    ? token
                    : throw new SyntaxErrorException($"Unexpected character {memory.Span[0]}");
        }
    }

}