using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using InventoryReservation.Application.Interfaces;
using InventoryReservation.Application.Common.Exceptions;

namespace InventoryReservation.Application.Commands.AdjustQuantity
{
    public class AdjustQuantityCommandHandler : IRequestHandler<AdjustQuantityCommand, Unit>
    {
        private readonly IInventoryRepository _repository;
        public AdjustQuantityCommandHandler(IInventoryRepository repository) => _repository = repository;

        public async Task<Unit> Handle(AdjustQuantityCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.ItemId);
            if (item == null) throw new NotFoundException("Item not found");

            item.AdjustQuantity(request.Delta);
            await _repository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
