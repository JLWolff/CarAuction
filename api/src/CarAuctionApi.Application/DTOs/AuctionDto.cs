using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.DTOs
{
    public class AuctionDto
    {
        public string Id { get; set; } = string.Empty;
        
        public string VehicleId { get; set; } = string.Empty;
        
        public VehicleDto Vehicle { get; set; } = null!;
        
        public AuctionStatus Status { get; set; }
        
        public decimal CurrentHighestBid { get; set; }
        
        public string? HighestBidderId { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public List<BidDto> Bids { get; set; } = new();
    }
}
