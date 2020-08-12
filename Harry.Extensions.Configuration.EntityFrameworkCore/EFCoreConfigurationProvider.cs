using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class EFCoreConfigurationProvider : ConfigurationProvider
    {
        private readonly EFCoreConfigurationSource _source;
        public EFCoreConfigurationProvider(EFCoreConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }
        public override void Load()
        {
            using (ConfigurationDbContext db = new ConfigurationDbContext(_source.DbOptions))
            {
                db.Database.EnsureCreated();
                foreach (var item in db.Configurations)
                {
                    Set(item.Key, item.Value);
                }
            }
        }

        public override void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            base.Set(key, value);
            if (_source.Options.Filter == null || _source.Options.Filter.Invoke(key, value))
            {
                using (ConfigurationDbContext db = new ConfigurationDbContext(_source.DbOptions))
                {
                    db.Database.EnsureCreated();
                    var model = db.Configurations
                        .Where(m => m.Key == key)
                        .FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = value;
                    }
                    else
                    {
                        db.Configurations.Add(new ConfigurationEntity() { Key = key, Value = value });
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
