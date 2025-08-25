using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Interfaces;
using CarAuctionApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Infrastructure.Repositories
{
    public class VehiclesInventoryRepository : IVehiclesInventoryRepository
    {
        private readonly CarAuctionDbContext _context;

        public VehiclesInventoryRepository(CarAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> SearchAsync(VehicleType? type, string? manufacturer, string? model, int? year)
        {
            var query = _context.Vehicles.AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(v => v.Type == type.Value);
            }

            if (!string.IsNullOrWhiteSpace(manufacturer))
            {
                query = query.Where(v => v.Manufacturer.ToLower().Contains(manufacturer.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                query = query.Where(v => v.Model.ToLower().Contains(model.ToLower()));
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.Year == year.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Vehicles.AnyAsync(v => v.Id == id);
        }
    }
}
