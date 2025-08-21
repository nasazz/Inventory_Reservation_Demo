using System;
using MediatR;

namespace InventoryReservation.Domain.Events
{
    // Domain event marker that is also a MediatR notification
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
