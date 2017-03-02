using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Harry.Extensions.MicrosoftConfiguration.Consul
{
    public class ConsulConfigurationOptions
    {
        public Uri Address { get; set; }

        public X509Certificate2 ClientCertificate { get; set; }

        public string Datacenter { get; set; }

        public NetworkCredential HttpAuth { get; set; }

        public string Token { get; set; }

        public TimeSpan? WaitTime { get; set; }

        public string Prefix { get; set; }
    }
}
