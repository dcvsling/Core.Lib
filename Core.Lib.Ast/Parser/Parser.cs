using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        async public Task<IEnumerable<Node>> Parse(ReadOnlyMemory<char> source)
        {
            using (var context = new ParserContext(_parser))
            {
                return context.Parse((await _lexer.Lex(source)).ToArray().AsSpan());
            }
        }
    }
}