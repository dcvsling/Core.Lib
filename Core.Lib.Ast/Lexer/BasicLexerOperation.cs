using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.Ast.Abstractions;
using Core.Lib.Ast.Models;
using Core.Lib.Reflections;

namespace Core.Lib.Ast.Lexer
{


    internal class BasicLexerOperation : ILexerOperation
    {
        public Task<Token> GetTokenAsync(ReadOnlyMemory<char> memory)
        {
            Func<char, bool> condition = default;
            string name = Empty.String;
            foreach (var factory in _predicates)
            {
                if (null != (condition = factory.Key(memory.Span[0])))
                {
                    name = factory.Value;
                    break;
                }
            }
            var count = 1;
            while (memory.Length > count && condition(memory.Span[count])) { count++; }
            var val = memory.Slice(0, count).ToString();
            return Task.FromResult<Token>((string.IsNullOrWhiteSpace(name) ? val : name, val));
        }

        private const string WORD = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
        private const string NUMBER = "0123456789,_";
        private const string WORDANDNUMBER = WORD + NUMBER;

        private static Dictionary<Func<char, Func<char, bool>>, string> _predicates
            = new Dictionary<Func<char, Func<char, bool>>, string>
            {
                [first => WORD.IndexOf(first) >= 0 ? c => WORDANDNUMBER.IndexOf(c) >= 0 : default(Func<char, bool>)] = "Text",
                [first => NUMBER.IndexOf(first) >= 0 ? c => NUMBER.IndexOf(c) >= 0 : default(Func<char, bool>)] = "Number",
                [first => WORDANDNUMBER.IndexOf(first) < 0 ? c => c == first : default(Func<char, bool>)] = Empty.String,
            };
    }
}