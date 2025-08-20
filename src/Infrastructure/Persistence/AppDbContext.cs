using Microsoft.EntityFrameworkCore;
using InventoryReservation.Domain.Entities;

namespace InventoryReservation.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map InventoryItem - minimal mapping so EF works with our aggregate
            modelBuilder.Entity<InventoryItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Sku).IsRequired().HasMaxLength(64);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.QuantityOnHand).IsRequired();
                b.Property(x => x.ReservedQuantity).IsRequired();
            });
        }
    }
}
