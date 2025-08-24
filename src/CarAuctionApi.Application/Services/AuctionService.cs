using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Extensions;
using CarAuctionApi.Application.Interfaces;
using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Interfaces;

namespace CarAuctionApi.Application.Services
{ 
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IVehiclesInventoryRepository _vehiclesInventoryRepository;

        public AuctionService(IAuctionRepository auctionRepository, IVehiclesInventoryRepository vehiclesInventoryRepository)
        {
            _auctionRepository = auctionRepository;
            _vehiclesInventoryRepository = vehiclesInventoryRepository;
        }

        public async Task<AuctionDto> StartAuctionAsync(StartAuctionDto startAuctionDto)
        {
            if(string.IsNullOrEmpty(startAuctionDto.VehicleId))
            {
                throw new ArgumentException("Vehicle ID cannot be null or empty on auction start.");
            }
            
            var vehicle = await _vehiclesInventoryRepository.GetByIdAsync(startAuctionDto.VehicleId);
            if (vehicle is null)
            {
                throw new VehicleNotFoundException(startAuctionDto.VehicleId);
            }

            if (await _auctionRepository.HasActiveAuctionAsync(startAuctionDto.VehicleId))
            {
                throw new AuctionAlreadyActiveException(startAuctionDto.VehicleId);
            }

            var auction = new Auction(startAuctionDto.VehicleId, vehicle);
            auction.Start();
            
            await _auctionRepository.AddAsync(auction);

            return auction.MapToDto();
        }

        public async Task CloseAuctionAsync(string auctionId)
        {
            if(string.IsNullOrEmpty(auctionId))
            {
                throw new ArgumentException("Auction ID cannot be null or empty on auction close.");
            }
            
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
            
            if (auction is null or { Status: AuctionStatus.Closed })
            {
                return;
            }

            auction.Close();
            await _auctionRepository.UpdateAsync(auction);
        }

        public async Task<AuctionDto> PlaceBidAsync(string auctionId, CreateBidDto createBidDto)
        {
            if(string.IsNullOrEmpty(auctionId))
            {
                throw new ArgumentException("Auction ID cannot be null or empty on auction bidding.");
            }
            
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
            if (auction is null)
            {
                throw new AuctionNotFoundException(auctionId);
            }

            try
            {
                auction.PlaceBid(createBidDto.BidderId, createBidDto.Amount);
                await _auctionRepository.UpdateAsync(auction);
                return auction.MapToDto();
            }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            {
                throw new InvalidBidException(ex.Message);
            }
        }

        public async Task<AuctionDto?> GetAuctionByIdAsync(string auctionId)
        {
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
            return auction?.MapToDto();
        }

        public async Task<IEnumerable<AuctionDto>> GetActiveAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetActiveAuctionsAsync();
            return auctions.Select(x => x.MapToDto());
        }
    }
}
