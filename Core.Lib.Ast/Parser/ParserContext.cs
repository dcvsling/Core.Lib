namespace Core.Lib.Ast.Parser
{
    using Core.Lib.Ast.Abstractions;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserContext : IDisposable
    {
        private readonly IParserOperation _operation;

        public ParserContext(IParserOperation operation)
        {
            _operation = operation;
        }

        public IEnumerable<Node> Parse(IEnumerable<Token> tokens)
        {
            var nodes = new List<Node>();
            while (tokens.Any())
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