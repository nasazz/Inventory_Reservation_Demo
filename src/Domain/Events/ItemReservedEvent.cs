using System;

namespace InventoryReservation.Domain.Events
{
    public record ItemReservedEvent(Guid ItemId, int Quantity, DateTime OccurredOn) : IDomainEvent;
}
