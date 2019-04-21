using System;
using System.Threading.Tasks;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Lexer
{
    public interface IActionStore : IDisposable
    {
        Task<Func<char, bool>> Get(Behavior behavior);
    }
}