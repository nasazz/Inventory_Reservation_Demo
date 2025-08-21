using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using InventoryReservation.Application.Interfaces;
using InventoryReservation.Application.Common.Exceptions;

namespace InventoryReservation.Application.Commands.ReserveItem
{
    public class ReserveItemCommandHandler : IRequestHandler<ReserveItemCommand, Unit>
    {
        private readonly IInventoryRepository _repository;

        public ReserveItemCommandHandler(IInventoryRepository repository) => _repository = repository;

        public async Task<Unit> Handle(ReserveItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.ItemId);
            if (item == null) throw new NotFoundException("Item not found");

            item.Reserve(request.Quantity);
            await _repository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
