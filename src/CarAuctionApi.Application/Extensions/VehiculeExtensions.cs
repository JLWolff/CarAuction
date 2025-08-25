using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.Extensions;

public static class VehicleExtensions
{
    internal static VehicleDto ToDto(this Vehicle v) => v switch
    {
        Sedan s => new VehicleDto
        {
            Id = s.Id,
            Type = VehicleType.Sedan,
            Manufacturer = s.Manufacturer,
            Model = s.Model,
            Year = s.Year,
            StartingBid = s.StartingBid,
            NumberOfDoors = s.NumberOfDoors,          
            NumberOfSeats = null,
            LoadCapacity = null
        },

        Hatchback h => new VehicleDto
        {
            Id = h.Id,
            Type = VehicleType.Hatchback,
            Manufacturer = h.Manufacturer,
            Model = h.Model,
            Year = h.Year,
            StartingBid = h.StartingBid,
            NumberOfDoors = h.NumberOfDoors,
            NumberOfSeats = null,
            LoadCapacity = null
        },

        SUV u => new VehicleDto
        {
            Id = u.Id,
            Type = VehicleType.SUV,
            Manufacturer = u.Manufacturer,
            Model = u.Model,
            Year = u.Year,
            StartingBid = u.StartingBid,
            NumberOfDoors = null,
            NumberOfSeats = u.NumberOfSeats,         
            LoadCapacity = null
        },

        Truck t => new VehicleDto
        {
            Id = t.Id,
            Type = VehicleType.Truck,
            Manufacturer = t.Manufacturer,
            Model = t.Model,
            Year = t.Year,
            StartingBid = t.StartingBid,
            NumberOfDoors = null,
            NumberOfSeats = null,
            LoadCapacity = t.LoadCapacity   
        },

        _ => throw new ArgumentException($"Unsupported vehicle type: {v.GetType().Name}")
    };
}