using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Services;
using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace CarAuctionApi.Tests.Application
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepo;
        private readonly Mock<IVehiclesInventoryRepository> _vehicleRepo;
        private readonly AuctionService _sut;

        public AuctionServiceTests()
        {
            _auctionRepo = new Mock<IAuctionRepository>();
            _vehicleRepo = new Mock<IVehiclesInventoryRepository>();
            _sut = new AuctionService(_auctionRepo.Object, _vehicleRepo.Object);
        }

        [Fact]
        public async Task StartAuctionAsync_WhenValidRequest_StartsAuctionAndReturnsDto()
        {
            // Arrange
            var startAuctionDto = new StartAuctionDto { VehicleId = "v1" };
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);

            _vehicleRepo.Setup(r => r.GetByIdAsync(startAuctionDto.VehicleId))
                       .ReturnsAsync(vehicle);
            _auctionRepo.Setup(r => r.HasActiveAuctionAsync(startAuctionDto.VehicleId))
                       .ReturnsAsync(false);
            _auctionRepo.Setup(r => r.AddAsync(It.IsAny<Auction>()))
                       .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.StartAuctionAsync(startAuctionDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(startAuctionDto.VehicleId, result.VehicleId);
            Assert.Equal(AuctionStatus.Active, result.Status);
            Assert.Equal(vehicle.StartingBid, result.CurrentHighestBid);
            Assert.Null(result.HighestBidderId);
            Assert.True(result.StartTime > DateTime.MinValue);
            Assert.Null(result.EndTime);
            Assert.Empty(result.Bids);

            _vehicleRepo.Verify(r => r.GetByIdAsync(startAuctionDto.VehicleId), Times.Once);
            _auctionRepo.Verify(r => r.HasActiveAuctionAsync(startAuctionDto.VehicleId), Times.Once);
            _auctionRepo.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task StartAuctionAsync_WhenVehicleIdIsNullOrEmpty_ThrowsArgumentException(string vehicleId)
        {
            // Arrange
            var startAuctionDto = new StartAuctionDto { VehicleId = vehicleId! };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.StartAuctionAsync(startAuctionDto));
            Assert.Equal("Vehicle ID cannot be null or empty on auction start.", exception.Message);

            _vehicleRepo.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _auctionRepo.Verify(r => r.HasActiveAuctionAsync(It.IsAny<string>()), Times.Never);
            _auctionRepo.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task StartAuctionAsync_WhenVehicleNotFound_ThrowsVehicleNotFoundException()
        {
            // Arrange
            var startAuctionDto = new StartAuctionDto { VehicleId = "v1" };

            _vehicleRepo.Setup(r => r.GetByIdAsync(startAuctionDto.VehicleId))
                       .ReturnsAsync((Vehicle?)null);

            // Act & Assert
            await Assert.ThrowsAsync<VehicleNotFoundException>(() => _sut.StartAuctionAsync(startAuctionDto));

            _vehicleRepo.Verify(r => r.GetByIdAsync(startAuctionDto.VehicleId), Times.Once);
            _auctionRepo.Verify(r => r.HasActiveAuctionAsync(It.IsAny<string>()), Times.Never);
            _auctionRepo.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task StartAuctionAsync_WhenAuctionAlreadyActive_ThrowsAuctionAlreadyActiveException()
        {
            // Arrange
            var startAuctionDto = new StartAuctionDto { VehicleId = "v1" };
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);

            _vehicleRepo.Setup(r => r.GetByIdAsync(startAuctionDto.VehicleId))
                       .ReturnsAsync(vehicle);
            _auctionRepo.Setup(r => r.HasActiveAuctionAsync(startAuctionDto.VehicleId))
                       .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<AuctionAlreadyActiveException>(() => _sut.StartAuctionAsync(startAuctionDto));

            _vehicleRepo.Verify(r => r.GetByIdAsync(startAuctionDto.VehicleId), Times.Once);
            _auctionRepo.Verify(r => r.HasActiveAuctionAsync(startAuctionDto.VehicleId), Times.Once);
            _auctionRepo.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task CloseAuctionAsync_WhenAuctionExists_ClosesAuction()
        {
            // Arrange
            var auctionId = "a1";
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);
            _auctionRepo.Setup(r => r.UpdateAsync(It.IsAny<Auction>()))
                       .Returns(Task.CompletedTask);

            // Act
            await _sut.CloseAuctionAsync(auctionId);

            // Assert
            Assert.Equal(AuctionStatus.Closed, auction.Status);
            Assert.NotNull(auction.EndTime);

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(auction), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CloseAuctionAsync_WhenAuctionIdIsNullOrEmpty_ThrowsArgumentException(string auctionId)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CloseAuctionAsync(auctionId!));
            Assert.Equal("Auction ID cannot be null or empty on auction close.", exception.Message);

            _auctionRepo.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task CloseAuctionAsync_WhenAuctionNotFound_DoesNothing()
        {
            // Arrange
            var auctionId = "a1";

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync((Auction?)null);

            // Act
            await _sut.CloseAuctionAsync(auctionId);

            // Assert
            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task CloseAuctionAsync_WhenAuctionAlreadyClosed_DoesNothing()
        {
            // Arrange
            var auctionId = "a1";
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();
            auction.Close(); // Already closed

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);

            // Act
            await _sut.CloseAuctionAsync(auctionId);

            // Assert
            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task PlaceBidAsync_WhenValidBid_PlacesBidAndReturnsUpdatedAuction()
        {
            // Arrange
            var auctionId = "a1";
            var createBidDto = new CreateBidDto { BidderId = "bidder1", Amount = 16000m };
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);
            _auctionRepo.Setup(r => r.UpdateAsync(It.IsAny<Auction>()))
                       .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.PlaceBidAsync(auctionId, createBidDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createBidDto.Amount, result.CurrentHighestBid);
            Assert.Equal(createBidDto.BidderId, result.HighestBidderId);
            Assert.Single(result.Bids);
            Assert.Equal(createBidDto.Amount, result.Bids.First().Amount);
            Assert.Equal(createBidDto.BidderId, result.Bids.First().BidderId);

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(auction), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task PlaceBidAsync_WhenAuctionIdIsNullOrEmpty_ThrowsArgumentException(string auctionId)
        {
            // Arrange
            var createBidDto = new CreateBidDto { BidderId = "bidder1", Amount = 16000m };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.PlaceBidAsync(auctionId!, createBidDto));
            Assert.Equal("Auction ID cannot be null or empty on auction bidding.", exception.Message);

            _auctionRepo.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task PlaceBidAsync_WhenAuctionNotFound_ThrowsAuctionNotFoundException()
        {
            // Arrange
            var auctionId = "a1";
            var createBidDto = new CreateBidDto { BidderId = "bidder1", Amount = 16000m };

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync((Auction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<AuctionNotFoundException>(() => _sut.PlaceBidAsync(auctionId, createBidDto));

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task PlaceBidAsync_WhenBidAmountTooLow_ThrowsInvalidBidException()
        {
            // Arrange
            var auctionId = "a1";
            var createBidDto = new CreateBidDto { BidderId = "bidder1", Amount = 14000m }; 
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidBidException>(() => _sut.PlaceBidAsync(auctionId, createBidDto));

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task PlaceBidAsync_WhenAuctionNotActive_ThrowsInvalidBidException()
        {
            // Arrange
            var auctionId = "a1";
            var createBidDto = new CreateBidDto { BidderId = "bidder1", Amount = 16000m };
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle); // Not started, status is Draft

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidBidException>(() => _sut.PlaceBidAsync(auctionId, createBidDto));

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task PlaceBidAsync_WhenBidderIdIsEmpty_ThrowsInvalidBidException()
        {
            // Arrange
            var auctionId = "a1";
            var createBidDto = new CreateBidDto { BidderId = string.Empty, Amount = 16000m };
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidBidException>(() => _sut.PlaceBidAsync(auctionId, createBidDto));

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
            _auctionRepo.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task GetAuctionByIdAsync_WhenAuctionExists_ReturnsAuctionDto()
        {
            // Arrange
            var auctionId = "a1";
            var vehicle = new Sedan("v1", "Manufacturer", "Model", 2020, 15000m, 4);
            var auction = new Auction("v1", vehicle);
            auction.Start();

            _auctionRepo.Setup(r => r.GetByIdAsync(auctionId))
                       .ReturnsAsync(auction);

            // Act
            var result = await _sut.GetAuctionByIdAsync(auctionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(auction.Id, result.Id);
            Assert.Equal(auction.VehicleId, result.VehicleId);
            Assert.Equal(auction.Status, result.Status);
            Assert.Equal(auction.CurrentHighestBid, result.CurrentHighestBid);

            _auctionRepo.Verify(r => r.GetByIdAsync(auctionId), Times.Once);
        }
    }
}
