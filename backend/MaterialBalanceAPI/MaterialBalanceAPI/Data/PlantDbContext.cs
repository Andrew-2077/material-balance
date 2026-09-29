using Microsoft.EntityFrameworkCore;
using MaterialBalanceAPI.Data.Entities;

namespace MaterialBalanceAPI.Data
{
    public class PlantDbContext : DbContext
    {
        public PlantDbContext(DbContextOptions<PlantDbContext> options) : base(options)
        {
        }
        public DbSet<StreamEntity> Streams { get; set; }
        public DbSet<PeriodEntity> Periods { get; set; }
        public DbSet<MeasurementEntity> Measurements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка первичных ключей
            modelBuilder.Entity<StreamEntity>().HasKey(s => s.Id);
            modelBuilder.Entity<PeriodEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<MeasurementEntity>().HasKey(m => m.Id);
        }
    }
}
