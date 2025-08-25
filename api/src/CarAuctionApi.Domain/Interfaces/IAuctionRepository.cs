using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Domain.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction?> GetByIdAsync(string id);
        
        Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
        
        Task AddAsync(Auction auction);
        
        Task UpdateAsync(Auction auction);
        
        Task<bool> HasActiveAuctionAsync(string vehicleId);
    }
}
