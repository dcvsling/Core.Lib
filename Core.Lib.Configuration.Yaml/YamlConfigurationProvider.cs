using Microsoft.Extensions.Configuration;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Core.Lib.Configuration.Yaml
{
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(FileConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            var reader = new StreamReader(stream);
            var ys = new YamlStream();
            ys.Load(new Parser(reader));
            var visitor = new YamlConfigurationVisitor(Data);
            visitor.Visit(ys);
            reader.Dispose();
        }
    }
}
