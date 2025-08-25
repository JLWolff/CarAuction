using CarAuctionApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarAuctionApi.Application.DTOs
{
    public class VehicleDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Manufacturer is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Manufacturer name must be between 1 and 100 characters.")]
        public string Manufacturer { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Model is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Model name must be between 1 and 100 characters.")]
        public string Model { get; set; } = string.Empty;
        
        public int Year { get; set; }
        
        [Range(0.1, double.MaxValue, ErrorMessage = "Starting bid must be greater than 0.")]
        public decimal StartingBid { get; set; }
        
        [Range(2, 5, ErrorMessage = "Number of doors must be between 2 and 5.")]
        public int? NumberOfDoors { get; set; }
        
        [Range(2, 9, ErrorMessage = "Number of seats must be between 2 and 9.")]
        public int? NumberOfSeats { get; set; }
        
        [Range(0.1, double.MaxValue, ErrorMessage = "Load capacity must be greater than 0.")]
        public decimal? LoadCapacity { get; set; }
        
        [Required(ErrorMessage = "Vehicle type is required.")]
        public VehicleType Type { get; set; }
    }
}
