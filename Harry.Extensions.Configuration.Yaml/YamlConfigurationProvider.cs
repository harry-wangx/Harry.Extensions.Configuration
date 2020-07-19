using Microsoft.Extensions.Configuration;
using System.IO;

namespace Harry.Extensions.Configuration.Yaml
{
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(YamlConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            Data = YamlConfigurationFileParser.Parse(stream);
        }
    }
}
