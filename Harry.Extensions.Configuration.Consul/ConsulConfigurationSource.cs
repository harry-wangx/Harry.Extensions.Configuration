using Microsoft.Extensions.Configuration;

namespace Harry.Extensions.Configuration.Consul
{
    public class ConsulConfigurationSource : IConfigurationSource
    {

        public ConsulConfigurationOptions Options { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(Options);
        }
    }
}
