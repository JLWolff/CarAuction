namespace CarAuctionApi.Domain.Entities
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; set; }

        public Sedan(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
            Type = VehicleType.Sedan;
        }

        public Sedan() : base() { } // EntityFramework ctor
    }
}
