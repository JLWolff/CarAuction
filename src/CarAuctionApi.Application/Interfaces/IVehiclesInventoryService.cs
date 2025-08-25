using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.Interfaces;

public interface IVehiclesInventoryService
{
    Task<VehicleDto> AddVehicleAsync(VehicleDto vehicleDto);
    
    Task<IEnumerable<VehicleDto>> SearchVehiclesAsync(VehicleType? type, string? manufacturer, string? model, int? year);
    
}
