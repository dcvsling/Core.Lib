namespace Core.Lib.Ast.Parser
{
    using Core.Lib.Ast.Abstractions;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

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
        public Node Parse(IEnumerable<Token> span)
            => _last.Parse(span) ?? _next.Parse(span);

        internal ParserOperation Append(IParserOperation parser)
        {
            _next = parser;
            return new ParserOperation(this);
        }

        private class EmptyOperation : IParserOperation
        {
            public Node Parse(IEnumerable<Token> span)
                => throw new SyntaxErrorException($"Unexpected character {span.FirstOrDefault()}");
        }
    }
}