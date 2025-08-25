using System.ComponentModel.DataAnnotations;

namespace CarAuctionApi.Application.DTOs
{
    public class CreateBidDto
    {
        [Required(ErrorMessage = "Bidder ID is required.")]
        public string BidderId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Bid amout is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
