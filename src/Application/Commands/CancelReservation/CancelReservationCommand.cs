using System;
using MediatR;

namespace InventoryReservation.Application.Commands.CancelReservation
{
    public record CancelReservationCommand(Guid ItemId, int Quantity) : IRequest<Unit>;
}
