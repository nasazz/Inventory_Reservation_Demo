using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using InventoryReservation.Application.Interfaces;
using InventoryReservation.Domain.Entities;

namespace InventoryReservation.Application.Commands.CreateItem
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Guid>
    {
        private readonly IInventoryRepository _repository;

        public CreateItemCommandHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new InventoryItem(request.Id, request.Sku, request.Name, request.QuantityOnHand);
            await _repository.AddAsync(item);
            await _repository.SaveChangesAsync();
            return item.Id;
        }
    }
}
