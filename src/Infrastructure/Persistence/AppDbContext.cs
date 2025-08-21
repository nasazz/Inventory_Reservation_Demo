using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryReservation.Domain.Entities;
using InventoryReservation.Domain.Events;
using MediatR;

namespace InventoryReservation.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IMediator _mediator;
        public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Sku).IsRequired().HasMaxLength(64);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.QuantityOnHand).IsRequired();
                b.Property(x => x.ReservedQuantity).IsRequired();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // save first
            var result = await base.SaveChangesAsync(cancellationToken);

            // collect domain events
            var entitiesWithEvents = ChangeTracker.Entries<InventoryItem>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents != null && e.DomainEvents.Any())
                .ToList();

            var events = entitiesWithEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();

            // publish each via MediatR
            foreach (var @event in events)
            {
                await _mediator.Publish(@event, cancellationToken);
            }

            // clear events
            entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

            return result;
        }
    }
}
