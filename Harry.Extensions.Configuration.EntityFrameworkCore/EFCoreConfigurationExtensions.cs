using Harry.Extensions.Configuration.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class EFCoreConfigurationExtensions
    {
        public static IConfigurationBuilder AddEntityFramework(this IConfigurationBuilder builder
            , Action<DbContextOptionsBuilder> dbOptionsAction = null
            , Action<EFCoreConfigurationOptions> optionsAction = null)
        {
            DbContextOptionsBuilder dbOptionsBuilder = new DbContextOptionsBuilder();
            dbOptionsAction?.Invoke(dbOptionsBuilder);

            EFCoreConfigurationOptions options = new EFCoreConfigurationOptions();
            optionsAction?.Invoke(options);

            builder.Add(new EFCoreConfigurationSource(dbOptionsBuilder.Options, options));
            return builder;
        }
    }
}
