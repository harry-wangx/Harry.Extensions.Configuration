using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Extensions.MicrosoftConfiguration.Sqlite
{
    public class SqliteConfigurationSource: IConfigurationSource
    {
        public string ConnectionString { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SqliteConfigurationProvider(ConnectionString);
        }
    }
}
