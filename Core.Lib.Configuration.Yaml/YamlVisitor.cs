using Core.Lib.Reflections;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Core.Lib.Configuration.Yaml
{
    internal class YamlConfigurationVisitor : YamlVisitorBase
    {
        private IDictionary<string, string> Data { get; }
        private readonly Dictionary<string, object> _archors;
        private Stack<string> _path;
        private Stack<object> _buffer;
        public YamlConfigurationVisitor(IDictionary<string, string> data = default)
        {
            Data = data ?? new Dictionary<string, string>();
            _archors = new Dictionary<string, object>();
            _path = new Stack<string>();
            _buffer = new Stack<object>();
        }

        public override void Visit(YamlScalarNode scalar)
        {
            switch (scalar.Anchor)
            {
                case "&":
                    var visitor = new YamlConfigurationVisitor();
                    scalar.Accept(visitor);
                    _archors.Add(scalar.Value, visitor.Data);
                    break;
                case "*":
                    var archor = (Dictionary<string, object>)(_archors.ContainsKey(scalar.Value)
                        ? _archors[scalar.Value]
                        : throw new YamlException(scalar.Start, scalar.End, $"archor {scalar.Value} is not exist"));
                    if (scalar.Tag != "<<")
                        _path.Push(scalar.Tag);
                    archor.Each(x => Data.Add($"{CurrentPath}:{x.Key}", x.Value.ToString()));
                    if (scalar.Tag != "<<")
                        _path.Pop();
                    break;
                default:
                    _buffer.Push(scalar.Value);
                    break;
            }

        }

        protected override void VisitChildren(YamlSequenceNode sequence)
        {
            var count = 0;
            foreach (var child in sequence.Children)
            {
                _path.Push(count.ToString());
                child.Accept(this);
                if (_buffer.Any())
                    SaveData();
                _path.Pop();
                count++;
            }
        }
        protected override void VisitPair(YamlNode key, YamlNode value)
        {
            key.Accept(this);
            _path.Push(_buffer.Pop().ToString());
            value.Accept(this);
            if (_buffer.Any())
                SaveData();
            _path.Pop();
        }

        private string CurrentPath
            => string.Join(":", _path.Reverse());

        private void SaveData()
            => Data.Add(CurrentPath, _buffer.Pop().ToString());
    }
}
