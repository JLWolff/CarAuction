using System.ComponentModel.DataAnnotations;

namespace CarAuctionApi.Application.DTOs
{
    public class BidDto
    {
        public string Id { get; set; } = string.Empty;
        
        public string AuctionId { get; set; } = string.Empty;
        
        public string BidderId { get; set; } = string.Empty;
        
        public decimal Amount { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}
