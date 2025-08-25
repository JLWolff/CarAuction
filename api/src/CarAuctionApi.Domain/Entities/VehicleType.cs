using System.ComponentModel.DataAnnotations;

namespace CarAuctionApi.Domain.Entities
{
    public enum VehicleType
    {
        [Display(Name = "Sedan")]
        Sedan,

        [Display(Name = "SUV")]
        SUV,

        [Display(Name = "Hatchback")]
        Hatchback,

        [Display(Name = "Truck")]
        Truck
    }
}