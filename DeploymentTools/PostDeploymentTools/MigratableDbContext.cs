using Microsoft.EntityFrameworkCore;

namespace PostDeploymentTools
{
    public class MigratableDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _schemaName;

        public MigratableDbContext(string connectionString, string schemaName)
        {
            _connectionString = connectionString;
            _schemaName = schemaName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, x => x.MigrationsHistoryTable("__MigrationsHistory", _schemaName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_schemaName);
        }
    }
}
