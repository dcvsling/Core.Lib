using System.Collections.Generic;

namespace Core.Lib.Ast.Models
{
    public class AstOptions
    {
        public Dictionary<string, string> Behaviors { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public List<Operator> Operations { get; set; } = new List<Operator>();
    }
}
