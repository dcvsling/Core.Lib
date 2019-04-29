namespace Core.Lib.Ast.Parser
{
    using Core.Lib.Ast.Abstractions;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    internal class BasicParserOperation : IParserOperation
    {
        public Node Parse(IEnumerable<Token> span)
        {
            var i = 0;

            while (span.Any())
            {


                i++;
            }

            return default;
        }
    }
}