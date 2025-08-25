using System.ComponentModel.DataAnnotations;

namespace CarAuctionApi.Application.DTOs
{
    public class StartAuctionDto
    {
        [Required(ErrorMessage = "Vehicle ID is required.")]
        public string VehicleId { get; set; } = string.Empty;
    }
}
