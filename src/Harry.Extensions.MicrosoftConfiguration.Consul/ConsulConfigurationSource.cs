using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Extensions.MicrosoftConfiguration.Consul
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
