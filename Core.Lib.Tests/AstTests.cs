using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Lexer;
using Core.Lib.Ast.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Lib.Tests
{
    public class AstTests
    {
        [Fact]
        public void BasicOperationForRules()
        {
            var provider = new ServiceCollection().AddAst().AddLoader().Services
                .AddSingleton<ILexer, Lexer>()
                .AddSingleton<ILexerOperation, BasicLexerOperation>()
                .BuildServiceProvider();
            var lexer = provider.GetRequiredService<ILexer>();
            var ast = provider.GetRequiredService<IOptions<AstOptions>>().Value;
            var result = ast.Behaviors.Values.Select(x => x.AsMemory()).Select(lexer.Lex).Select(x => x.Result).ToArray();
        }


        [Fact]
        async public Task Test1()
        {

            var provider = new ServiceCollection().AddAst().Services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var tokens = await provider.GetRequiredService<ILexer>().Lex(Data.AsMemory());
        }

        private const string Data = @"
test:
 test2:
 - test3_1: a
 - test3_2: 234
close: ok
";
    }
}
