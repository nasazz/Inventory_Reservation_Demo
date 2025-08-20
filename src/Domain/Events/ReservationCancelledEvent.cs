using System;

namespace InventoryReservation.Domain.Events
{
    public record ReservationCancelledEvent(Guid ItemId, int Quantity, DateTime OccurredOn) : IDomainEvent;
}
