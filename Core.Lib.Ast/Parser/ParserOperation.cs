namespace Core.Lib.Ast.Parser
{
    using System;
    using Core.Lib.Ast.Abstractions;
    using Models;

    internal class ParserOperation : IParserOperation
    {
        private readonly IParserOperation _last;
        private IParserOperation _next;
        public static IParserOperation Root => new EmptyOperation();
        internal ParserOperation(IParserOperation last)
        {
            _last = last;
            _next = default;
        }
        public Node Parse(ReadOnlySpan<Token> span)
            => _last.Parse(span) ?? _next.Parse(span);

        internal ParserOperation Append(IParserOperation parser)
        {
            _next = parser;
            return new ParserOperation(this);
        }

        private class EmptyOperation : IParserOperation
        {
            public Node Parse(ReadOnlySpan<Token> span)
                => throw new SyntaxErrorException($"Unexpected character {span[0]}");
        }
    }
}