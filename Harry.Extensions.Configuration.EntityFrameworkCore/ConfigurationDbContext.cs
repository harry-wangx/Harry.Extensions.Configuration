using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions options) : base(options) { }

#if DEBUG
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
#endif

        public DbSet<ConfigurationEntity> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ConfigurationEntity>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasIndex(c => c.ConfigurationKey).HasName("ConfigurationKeyIndex").IsUnique();
                b.ToTable("Configurations");

                b.Property(c => c.ConfigurationKey).HasMaxLength(256);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.UseLoggerFactory(_loggerFactory);
#endif
        }
    }
}
