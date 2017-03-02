using Harry.Extensions.MicrosoftConfiguration.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    public static class SqliteConfigurationExtensions
    {
        //public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder builder)
        //{
        //    return AddConsul(builder, null);
        //}

        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder builder, string connectionString)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.Add(new SqliteConfigurationSource() { ConnectionString = connectionString });
            return builder;
        }
        public static IConfigurationBuilder AddSqlite(this IConfigurationBuilder builder, DbConnection connection, Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
        {
            return builder;
        }

        //public static IConfigurationBuilder AddSqlite<TContext>(this IConfigurationBuilder builder, string connectionString, Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null) where TContext : DbContext
        //{
        //    return builder;
        //}

        //public static IConfigurationBuilder AddSqlite<TContext>(this IConfigurationBuilder builder, DbConnection connection, Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null) where TContext : DbContext
        //{
        //    return builder;
        //}
    }
}
