using CarAuctionApi.Application.DTOs;

namespace CarAuctionApi.Application.Interfaces;

public interface IAuctionService
{
    Task<AuctionDto> StartAuctionAsync(StartAuctionDto startAuctionDto);
    
    Task CloseAuctionAsync(string auctionId);
    
    Task<AuctionDto> PlaceBidAsync(string auctionId, CreateBidDto createBidDto);
    
    Task<AuctionDto?> GetAuctionByIdAsync(string auctionId);
    
    Task<IEnumerable<AuctionDto>> GetActiveAuctionsAsync();
}