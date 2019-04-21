namespace Core.Lib.Ast.Parser
{
    using System;
    using System.Collections.Generic;
    using Core.Lib.Ast.Abstractions;
    using Models;

    public class ParserContext : IDisposable
    {
        private readonly IParserOperation _operation;

        public ParserContext(IParserOperation operation)
        {
            _operation = operation;
        }

        public IEnumerable<Node> Parse(ReadOnlySpan<Token> tokens)
        {
            var nodes = new List<Node>();
            while (tokens.Length > 0)
            {
                nodes.Add(_operation.Parse(tokens));
            }
            return nodes;
        }

        public void Dispose()
        {
        }
    }
}