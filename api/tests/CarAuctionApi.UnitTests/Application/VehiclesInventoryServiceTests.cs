using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Services;
using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace CarAuctionApi.Tests.Application
{
    public class VehiclesInventoryServiceTests
    {
        private readonly Mock<IVehiclesInventoryRepository> _repo;
        private readonly VehiclesInventoryService _sut;

        public VehiclesInventoryServiceTests()
        {
            _repo = new Mock<IVehiclesInventoryRepository>();
            _sut = new VehiclesInventoryService(_repo.Object);
        }

        [Fact]
        public async Task AddVehicleAsync_WhenIdDoesNotExist_AddsAndReturnsDto()
        {
            // Arrange
            var dto = new VehicleDto
            {
                Id = "v1",
                Type = VehicleType.Sedan,
                Manufacturer = "Manufacturer",
                Model = "Model",
                Year = 2020,
                StartingBid = 5000m,
                NumberOfDoors = 4
            };

            _repo.Setup(r => r.ExistsAsync(dto.Id)).ReturnsAsync(false);

            _repo.Setup(r => r.AddAsync(It.IsAny<Vehicle>()))
                 .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.AddVehicleAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Id, result.Id);
            Assert.Equal(dto.Type, result.Type);
            Assert.Equal(dto.Manufacturer, result.Manufacturer);
            Assert.Equal(dto.Model, result.Model);
            Assert.Equal(dto.Year, result.Year);
            Assert.Equal(dto.StartingBid, result.StartingBid);
            Assert.Equal(dto.NumberOfDoors, result.NumberOfDoors);

            _repo.Verify(r => r.ExistsAsync(dto.Id), Times.Once);
            _repo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
        }

        [Fact]
        public async Task AddVehicleAsync_WhenIdAlreadyExists_ThrowsVehicleAlreadyExistsException()
        {
            // Arrange
            var dto = new VehicleDto
            {
                Id = "v1",
                Type = VehicleType.Truck,
                Manufacturer = "Manufacturer",
                Model = "Model",
                Year = 2022,
                StartingBid = 10000m,
                LoadCapacity = 12000m
            };

            _repo.Setup(r => r.ExistsAsync(dto.Id)).ReturnsAsync(true);

            // Act + Assert
            await Assert.ThrowsAsync<VehicleAlreadyExistsException>(() => _sut.AddVehicleAsync(dto));

            _repo.Verify(r => r.ExistsAsync(dto.Id), Times.Once);
            _repo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task SearchVehiclesAsync_FiltersArePassedThrough_AndResultsAreMapped()
        {
            // Arrange
            var filterType = VehicleType.Hatchback;
            var filterManufacturer = "Manufacturer1";
            var filterModel = "Model1";
            var filterYear = 2018;

            var v1 = new Hatchback(
                id: "h1",
                manufacturer: "Manufacturer1",
                model: "Model1",
                year: 2018,
                startingBid: 3000m,
                numberOfDoors: 5);

            var v2 = new Hatchback(
                id: "h2",
                manufacturer: "Manufacturer1",
                model: "Model1",
                year: 2018,
                startingBid: 3200m,
                numberOfDoors: 3);

            _repo.Setup(r => r.SearchAsync(filterType, filterManufacturer, filterModel, filterYear))
                 .ReturnsAsync(new[] { v1, v2 });

            // Act
            var results = (await _sut.SearchVehiclesAsync(filterType, filterManufacturer, filterModel, filterYear)).ToList();

            // Assert
            Assert.Equal(2, results.Count);

            var dto1 = results.Single(r => r.Id == "h1");
            Assert.Equal(VehicleType.Hatchback, dto1.Type);
            Assert.Equal(5, dto1.NumberOfDoors);
            Assert.Null(dto1.NumberOfSeats);
            Assert.Null(dto1.LoadCapacity);

            var dto2 = results.Single(r => r.Id == "h2");
            Assert.Equal(3, dto2.NumberOfDoors);

            _repo.Verify(r => r.SearchAsync(filterType, filterManufacturer, filterModel, filterYear), Times.Once);
        }

        [Fact]
        public async Task SearchVehiclesAsync_WhenNoResults_ReturnsEmpty()
        {
            // Arrange
            _repo.Setup(r => r.SearchAsync(null, null, null, null))
                 .ReturnsAsync(Array.Empty<Vehicle>());

            // Act
            var results = await _sut.SearchVehiclesAsync(null, null, null, null);

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
            _repo.Verify(r => r.SearchAsync(null, null, null, null), Times.Once);
        }
    }
}
