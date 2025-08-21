using System;
using MediatR;

namespace InventoryReservation.Application.Commands.AdjustQuantity
{
    public record AdjustQuantityCommand(Guid ItemId, int Delta) : IRequest<Unit>;
}
