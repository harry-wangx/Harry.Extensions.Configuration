using Microsoft.Extensions.Configuration;
using System.IO;

namespace Harry.Extensions.Configuration.Yaml
{
    public class YamlStreamConfigurationProvider : StreamConfigurationProvider
    {
        public YamlStreamConfigurationProvider(YamlStreamConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            Data = YamlConfigurationFileParser.Parse(stream);
        }
    }
}
