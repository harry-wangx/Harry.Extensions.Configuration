using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Harry.Extensions.Configuration.Consul
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
