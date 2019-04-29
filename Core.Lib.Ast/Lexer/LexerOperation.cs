namespace Core.Lib.Ast.Lexer
{
    using Abstractions;
    internal class LexerOperation : ILexerOperation, IChainableOperation
    {
        private readonly ILexerOperation _next;
        private ILexerOperation _last;

        public static IChainableOperation Root => new LexerOperation();
        private LexerOperation()
        {
            _next = default;
            _last = default;
        }
        internal LexerOperation(ILexerOperation last)
        {
            _next = last;
            _last = default;
        }

        public void GetToken(LexerOperationContext context)
        {
            _last?.GetToken(context);
            if (!context.IsSuccess) _next?.GetToken(context);
        }
        IChainableOperation IChainableOperation.Append(ILexerOperation operation)
        {
            _last = operation;
            return new LexerOperation(this);
        }
    }
}