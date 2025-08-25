using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Domain.Interfaces
{
    public interface IVehiclesInventoryRepository
    {
        Task<Vehicle?> GetByIdAsync(string id);
        
        Task<IEnumerable<Vehicle>> SearchAsync(VehicleType? type, string? manufacturer, string? model, int? year);
        
        Task AddAsync(Vehicle vehicle);
        
        Task<bool> ExistsAsync(string id);
    }
}
