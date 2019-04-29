using Core.Lib.Ast.Abstractions;
using Core.Lib.Reflections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Xunit;

namespace Core.Lib.Tests
{

    public class AstTests
    {
        //[Fact]
        //public void BasicOperationForRules()
        //{
        //    var provider = new ServiceCollection().AddAst().AddLoader().Services
        //        .AddSingleton<ILexer, Lexer>()
        //        .AddSingleton<ILexerOperation, BasicLexerOperation>()
        //        .BuildServiceProvider();
        //    var lexer = provider.GetRequiredService<ILexer>();
        //    var ast = provider.GetRequiredService<IOptions<AstOptions>>().Value;
        //    var result = ast.Behaviors.Values.Select(x => x.AsMemory())
        //        .Select(lexer.Lex)
        //        .SelectMany(x => x.Result)
        //        .ToArray();

        //}


        [Theory]
        [InlineData(new object[] { @".\assets\Ast\source.yml", @".\assets\Ast\result.yml" })]
        public void test_basic_lexer(string inputPath, string outputPath)
        {
            outputPath = outputPath.WithAssemblyDir();
            var Data = File.ReadAllText(inputPath.WithAssemblyDir());
            var provider = new ServiceCollection()
                .AddAst()
                .AddDefaultLexer()
                .Services
                .AddLogging(b => b
                    .AddConsole()
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider();
            var tokens = provider.GetRequiredService<ILexer>().Lex(Data).ToArray();
            if (!File.Exists(outputPath))
                File.WriteAllText(outputPath, tokens.ToYaml());
            var expect = File.ReadAllText(outputPath);
            Assert.Equal(expect, tokens.ToYaml());
        }
    }
}
