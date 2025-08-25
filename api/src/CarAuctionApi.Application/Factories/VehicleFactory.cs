using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.Factories;

public static class VehicleFactory
{
    public static Vehicle Create(VehicleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("Id is required.", nameof(dto.Id));
        }
        
        if (string.IsNullOrWhiteSpace(dto.Manufacturer))
        {
            throw new ArgumentException("Manufacturer is required.", nameof(dto.Manufacturer));
        }
        
        if (string.IsNullOrWhiteSpace(dto.Model))
        {
            throw new ArgumentException("Model is required.", nameof(dto.Model));
        }
        
        var currentYear = DateTime.UtcNow.Year;
        
        if (dto.Year > currentYear + 1)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.Year));
        }
        
        if (dto.StartingBid < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.StartingBid));
        }

        return dto.Type switch
        {
            VehicleType.Hatchback => CreateHatchback(dto),
            VehicleType.Sedan     => CreateSedan(dto),
            VehicleType.SUV       => CreateSuv(dto),
            VehicleType.Truck     => CreateTruck(dto),
            _ => throw new ArgumentOutOfRangeException(nameof(dto.Type))
        };
    }

    private static Hatchback CreateHatchback(VehicleDto dto)
    {
        if (dto.NumberOfDoors is null)
        {
            throw new ArgumentException("Number of doors is required for hatchback.");
        }
        
        var numberOfDoors = dto.NumberOfDoors.Value;
        
        if (numberOfDoors < 2 || numberOfDoors > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.NumberOfDoors));
        }
        
        return new Hatchback(dto.Id, dto.Manufacturer, dto.Model, dto.Year, dto.StartingBid, numberOfDoors);
    }

    private static Sedan CreateSedan(VehicleDto dto)
    {
        if (dto.NumberOfDoors is null)
        {
            throw new ArgumentException("Number of doors is required for sedan.");
        }
        
        var numberOfDoors = dto.NumberOfDoors.Value;
        
        if (numberOfDoors < 2 || numberOfDoors > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.NumberOfDoors));
        }
        
        return new Sedan(dto.Id, dto.Manufacturer, dto.Model, dto.Year, dto.StartingBid, numberOfDoors);
    }

    private static SUV CreateSuv(VehicleDto dto)
    {
        if (dto.NumberOfSeats is null)
        {
            throw new ArgumentException("Number of seats is required for SUV.");
        }
        
        var numberOfSeats = dto.NumberOfSeats.Value;
        
        if (numberOfSeats < 2 || numberOfSeats > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.NumberOfSeats));
        }
        
        return new SUV(dto.Id, dto.Manufacturer, dto.Model, dto.Year, dto.StartingBid, numberOfSeats);
    }

    private static Truck CreateTruck(VehicleDto dto)
    {
        if (dto.LoadCapacity is null)
        {
            throw new ArgumentException("Load capacity is required for truck.");
        }
        
        if (dto.LoadCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.LoadCapacity));
        }
        
        return new Truck(dto.Id, dto.Manufacturer, dto.Model, dto.Year, dto.StartingBid, dto.LoadCapacity.Value);
    }
}
