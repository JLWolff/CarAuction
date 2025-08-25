using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Extensions;
using CarAuctionApi.Application.Factories;
using CarAuctionApi.Application.Interfaces;
using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Interfaces;

namespace CarAuctionApi.Application.Services
{
    public class VehiclesInventoryService : IVehiclesInventoryService
    {
        private readonly IVehiclesInventoryRepository _vehiclesInventoryRepository;

        public VehiclesInventoryService(IVehiclesInventoryRepository vehiclesInventoryRepository)
        {
            _vehiclesInventoryRepository = vehiclesInventoryRepository;
        }

        public async Task<VehicleDto> AddVehicleAsync(VehicleDto vehicleDto)
        {
            if (await _vehiclesInventoryRepository.ExistsAsync(vehicleDto.Id))
            {
                throw new VehicleAlreadyExistsException(vehicleDto.Id);
            }
            
            var vehicle = VehicleFactory.Create(vehicleDto);
            await _vehiclesInventoryRepository.AddAsync(vehicle);      
            
            return vehicle.ToDto();
        }

        public async Task<IEnumerable<VehicleDto>> SearchVehiclesAsync(VehicleType? type, string? manufacturer, string? model, int? year)
        {
            var vehicles = await _vehiclesInventoryRepository.SearchAsync(type, manufacturer, model, year);
            return vehicles.Select(vehicle => vehicle.ToDto());
        }
    }
}
