using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryReservation.Application.Interfaces;
using InventoryReservation.Domain.Entities;
using InventoryReservation.Infrastructure.Persistence;

namespace InventoryReservation.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;
        public InventoryRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(InventoryItem item) => await _db.InventoryItems.AddAsync(item);
        public async Task<InventoryItem?> GetByIdAsync(Guid id) => await _db.InventoryItems.FindAsync(id);
        public async Task<InventoryItem?> GetBySkuAsync(string sku) => await _db.InventoryItems.FirstOrDefaultAsync(i => i.Sku == sku);
        public async Task<IEnumerable<InventoryItem>> ListAsync() => await _db.InventoryItems.ToListAsync();
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
