using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class EFCoreConfigurationSource : IConfigurationSource
    {
        public EFCoreConfigurationSource(DbContextOptions dbOptions, EFCoreConfigurationOptions options)
        {
            this.DbOptions = dbOptions ?? throw new ArgumentNullException(nameof(dbOptions));
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFCoreConfigurationProvider(this);
        }

        public DbContextOptions DbOptions { get; }

        public EFCoreConfigurationOptions Options { get; }
    }
}