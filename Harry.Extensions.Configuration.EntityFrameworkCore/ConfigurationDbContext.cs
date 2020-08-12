using Microsoft.EntityFrameworkCore;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ConfigurationEntity> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ConfigurationEntity>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasIndex(c => c.Key).HasName("KeyIndex").IsUnique();
                b.ToTable("Configurations");

                b.Property(c => c.Key).HasMaxLength(256);
            });
        }
    }
}
