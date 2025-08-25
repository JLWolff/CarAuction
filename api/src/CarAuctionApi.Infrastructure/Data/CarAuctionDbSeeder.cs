using CarAuctionApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Infrastructure.Data
{
    public static class CarAuctionDbSeeder
    {
        public static async Task SeedAsync(CarAuctionDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            if (await db.Vehicles.AnyAsync())
            {
                return;
            }

            var vehicles = new Vehicle[]
            {
                new Sedan(id: "v1", manufacturer: "Toyota", model: "Yaris", year: 2020, startingBid: 5000m, numberOfDoors: 4),
                new Hatchback(id: "v2", manufacturer: "Volkswagen", model: "Golf", year: 2018, startingBid: 3000m, numberOfDoors: 5),
                new SUV(id: "v3", manufacturer: "Honda", model: "CR-V", year: 2021, startingBid: 7000m, numberOfSeats: 5),
                new Sedan(id: "v5", manufacturer: "Hyundai",model: "330i", year: 2019, startingBid: 8000m, numberOfDoors: 4),
            };

            db.Vehicles.AddRange(vehicles);
            await db.SaveChangesAsync();
        }
    }
}