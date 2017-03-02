using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Extensions.MicrosoftConfiguration.Sqlite
{
    public class SqliteConfigurationProvider : ConfigurationProvider
    {
        private readonly string connectionString;

        public SqliteConfigurationProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            List<ConfigInfo> configs = null;
            using (ConfigurationDbContext db = new ConfigurationDbContext(connectionString))
            {
                configs = db.Configs.ToList();
            }
            if (configs != null && configs.Count > 0)
            {
                foreach (var item in configs)
                {
                    Set(item.Key, item.Value);
                }
            }
        }
    }
}
