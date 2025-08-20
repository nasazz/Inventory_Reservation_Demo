using System;
using System.Collections.Generic;
using InventoryReservation.Domain.Events;

namespace InventoryReservation.Domain.Entities
{
    // Simple aggregate root
    public class InventoryItem
    {
        public Guid Id { get; private set; }
        public string Sku { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public int QuantityOnHand { get; private set; }
        public int ReservedQuantity { get; private set; }

        // Domain events attached to the aggregate instance
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private InventoryItem() { } // EF
        public InventoryItem(Guid id, string sku, string name, int quantityOnHand)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));
            if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("Sku required", nameof(sku));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
            if (quantityOnHand < 0) throw new ArgumentOutOfRangeException(nameof(quantityOnHand));

            Id = id;
            Sku = sku;
            Name = name;
            QuantityOnHand = quantityOnHand;
            ReservedQuantity = 0;
        }

        public void AdjustQuantity(int delta)
        {
            var newQty = QuantityOnHand + delta;
            if (newQty < 0) throw new InvalidOperationException("Quantity cannot be negative");
            QuantityOnHand = newQty;
        }

        public void Reserve(int qty)
        {
            if (qty <= 0) throw new ArgumentException("Reserve quantity must be positive", nameof(qty));
            if (qty > AvailableQuantity()) throw new InvalidOperationException("Not enough available quantity to reserve");

            ReservedQuantity += qty;
            _domainEvents.Add(new ItemReservedEvent(Id, qty, DateTime.UtcNow));
        }

        public void CancelReservation(int qty)
        {
            if (qty <= 0) throw new ArgumentException("Cancel quantity must be positive", nameof(qty));
            if (qty > ReservedQuantity) throw new InvalidOperationException("Cancel quantity exceeds reserved");
            ReservedQuantity -= qty;
            _domainEvents.Add(new ReservationCancelledEvent(Id, qty, DateTime.UtcNow));
        }

        public int AvailableQuantity() => QuantityOnHand - ReservedQuantity;

        // helper to clear events after they are dispatched by infrastructure
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
