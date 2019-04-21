namespace Core.Lib.Ast.Lexer
{
    using System;
    using System.Threading.Tasks;
    using Abstractions;
    using Models;

    internal class OperatorLexerOperation : ILexerOperation
    {
        private readonly Operator _operator;
        private readonly IActionStore _actions;

        public OperatorLexerOperation(Operator @operator, IActionStore actions)
        {
            _actions = actions;
            _operator = @operator;
        }
        async public Task<Token> GetTokenAsync(ReadOnlyMemory<char> memory)
        {
            if (!(await _actions.Get(_operator.Check)).Invoke(memory.Span[0])) return default;
            var condition = await _actions.Get(_operator.Take);
            var count = 1;
            while (memory.Length > count && condition(memory.Span[count])) { count++; }
            var val = memory.Slice(0, count).ToString();
            return (string.IsNullOrWhiteSpace(_operator.Name) ? val : _operator.Name, val);

        }
    }
}