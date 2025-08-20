using System;

namespace InventoryReservation.Domain.Events
{
    public interface IDomainEvent { DateTime OccurredOn { get; } }
}
