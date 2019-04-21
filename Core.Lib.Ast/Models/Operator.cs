namespace Core.Lib.Ast.Models
{
    public class Operator
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public Behavior Check { get; set; }
        public Behavior Take { get; set; }
    }
}
