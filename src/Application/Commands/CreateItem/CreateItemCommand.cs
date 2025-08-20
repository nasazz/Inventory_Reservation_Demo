using System;
using MediatR;

namespace InventoryReservation.Application.Commands.CreateItem
{
    public record CreateItemCommand(Guid Id, string Sku, string Name, int QuantityOnHand) : IRequest<Guid>;
}
