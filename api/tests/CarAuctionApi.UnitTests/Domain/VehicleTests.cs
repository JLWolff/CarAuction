using CarAuctionApi.Domain.Entities;
using Xunit;

namespace CarAuctionApi.Tests.Domain
{
    public class VehicleTests
    {
        [Fact]
        public void Sedan_Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = "id";
            var manufacturer = "manufacturer";
            var model = "model";
            var year = 2023;
            var startingBid = 25000m;
            var numberOfDoors = 4;

            // Act
            var sedan = new Sedan(id, manufacturer, model, year, startingBid, numberOfDoors);

            // Assert
            Assert.Equal(id, sedan.Id);
            Assert.Equal(manufacturer, sedan.Manufacturer);
            Assert.Equal(model, sedan.Model);
            Assert.Equal(year, sedan.Year);
            Assert.Equal(startingBid, sedan.StartingBid);
            Assert.Equal(numberOfDoors, sedan.NumberOfDoors);
            Assert.Equal(VehicleType.Sedan, sedan.Type);
        }

        [Fact]
        public void SUV_Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = "id";
            var manufacturer = "manufacturer";
            var model = "model";
            var year = 2023;
            var startingBid = 35000m;
            var numberOfSeats = 7;

            // Act
            var suv = new SUV(id, manufacturer, model, year, startingBid, numberOfSeats);

            // Assert
            Assert.Equal(id, suv.Id);
            Assert.Equal(manufacturer, suv.Manufacturer);
            Assert.Equal(model, suv.Model);
            Assert.Equal(year, suv.Year);
            Assert.Equal(startingBid, suv.StartingBid);
            Assert.Equal(numberOfSeats, suv.NumberOfSeats);
            Assert.Equal(VehicleType.SUV, suv.Type);
        }

        [Fact]
        public void Truck_Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = "t1";
            var manufacturer = "manufacturer";
            var model = "model";
            var year = 2023;
            var startingBid = 40000m;
            var loadCapacity = 2000m;

            // Act
            var truck = new Truck(id, manufacturer, model, year, startingBid, loadCapacity);

            // Assert
            Assert.Equal(id, truck.Id);
            Assert.Equal(manufacturer, truck.Manufacturer);
            Assert.Equal(model, truck.Model);
            Assert.Equal(year, truck.Year);
            Assert.Equal(startingBid, truck.StartingBid);
            Assert.Equal(loadCapacity, truck.LoadCapacity);
            Assert.Equal(VehicleType.Truck, truck.Type);
        }

        [Fact]
        public void Hatchback_Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = "h1";
            var manufacturer = "manufacturer";
            var model = "model";
            var year = 2023;
            var startingBid = 22000m;
            var numberOfDoors = 5;

            // Act
            var hatchback = new Hatchback(id, manufacturer, model, year, startingBid, numberOfDoors);

            // Assert
            Assert.Equal(id, hatchback.Id);
            Assert.Equal(manufacturer, hatchback.Manufacturer);
            Assert.Equal(model, hatchback.Model);
            Assert.Equal(year, hatchback.Year);
            Assert.Equal(startingBid, hatchback.StartingBid);
            Assert.Equal(numberOfDoors, hatchback.NumberOfDoors);
            Assert.Equal(VehicleType.Hatchback, hatchback.Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Vehicle_Constructor_WithInvalidId_ShouldThrowArgumentNullException(string invalidId)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new Sedan(invalidId, "manufacturer", "model", 2023, 25000m, 4));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Vehicle_Constructor_WithInvalidManufacturer_ShouldThrowArgumentNullException(string invalidManufacturer)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new Sedan("s1", invalidManufacturer, "model", 2023, 25000m, 4));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Vehicle_Constructor_WithInvalidModel_ShouldThrowArgumentNullException(string invalidModel)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new Sedan("s1", "manufacturer", invalidModel, 2023, 25000m, 4));
        }
    }
}
