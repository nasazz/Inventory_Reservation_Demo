using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryReservation.Domain.Entities;

namespace InventoryReservation.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task AddAsync(InventoryItem item);
        Task<InventoryItem?> GetByIdAsync(Guid id);
        Task<InventoryItem?> GetBySkuAsync(string sku);
        Task<IEnumerable<InventoryItem>> ListAsync();
        Task SaveChangesAsync();
    }
}
