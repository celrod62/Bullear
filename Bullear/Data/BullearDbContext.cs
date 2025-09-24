using Microsoft.EntityFrameworkCore;
using BullearApp.Models;

namespace BullearApp.Data
{
    public class BullearDbContext : DbContext
    {
        public BullearDbContext(DbContextOptions<BullearDbContext> options) : base(options)
        {
        }

        public DbSet<Trade> Trades { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<FidelityUpload> FidelityUploads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Trade entity
            modelBuilder.Entity<Trade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.StrikePrice).HasPrecision(18, 2);
                entity.Property(e => e.Commission).HasPrecision(18, 2);
                entity.Property(e => e.Fee).HasPrecision(18, 2);
            });

            // Configure Position entity
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Return).HasPrecision(18, 2);
                entity.Property(e => e.AvgEntryPrice).HasPrecision(18, 2);
                entity.Property(e => e.AvgExitPrice).HasPrecision(18, 2);
            });

            // Configure FidelityUpload entity
            modelBuilder.Entity<FidelityUpload>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Commission).HasPrecision(18, 2);
                entity.Property(e => e.Fees).HasPrecision(18, 2);
                entity.Property(e => e.AccruedInterest).HasPrecision(18, 2);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.CashBalance).HasPrecision(18, 2);
            });
        }
    }
}
