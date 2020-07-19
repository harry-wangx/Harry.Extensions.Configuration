using Microsoft.Extensions.Configuration;

namespace Harry.Extensions.Configuration.Yaml
{
    public class YamlStreamConfigurationSource : StreamConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
            => new YamlStreamConfigurationProvider(this);
    }
}
