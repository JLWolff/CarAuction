using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Interfaces;
using CarAuctionApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly CarAuctionDbContext _context;

        public AuctionRepository(CarAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Auction?> GetByIdAsync(string id)
        {
            return await _context.Auctions
                .Include(a => a.Vehicle)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Include(a => a.Vehicle)
                .Include(a => a.Bids)
                .Where(a => a.Status == AuctionStatus.Active)
                .ToListAsync();
        }

        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasActiveAuctionAsync(string vehicleId)
        {
            return await _context.Auctions
                .AnyAsync(a => a.VehicleId == vehicleId && a.Status == AuctionStatus.Active);
        }
    }
}
