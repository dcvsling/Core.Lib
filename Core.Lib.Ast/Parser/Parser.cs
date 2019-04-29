using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Lib.Ast.Parser
{
    public class Parser
    {
        private readonly IParserOperation _parser;
        private readonly ILexer _lexer;

        public Parser(IParserOperation parser, ILexer lexer)
        {
            _parser = parser;
            _lexer = lexer;
        }

        async public Task<IEnumerable<Node>> Parse(string source)
        {
            using (var context = new ParserContext(_parser))
            {
                return context.Parse(_lexer.Lex(source));
            }
        }
    }
}