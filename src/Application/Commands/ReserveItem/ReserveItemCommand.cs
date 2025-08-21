using System;
using MediatR;

namespace InventoryReservation.Application.Commands.ReserveItem
{
    public record ReserveItemCommand(Guid ItemId, int Quantity) : IRequest<Unit>;
}
