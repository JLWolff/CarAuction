namespace CarAuctionApi.Domain.Entities
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; set; }

        public Hatchback(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
            Type = VehicleType.Hatchback;
        }

        public Hatchback() : base() { } // EntityFramework ctor
    }
}
