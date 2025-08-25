namespace CarAuctionApi.Domain.Entities
{
    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; set; }

        public Truck(string id, string manufacturer, string model, int year, decimal startingBid, decimal loadCapacity)
            : base(id, manufacturer, model, year, startingBid)
        {
            LoadCapacity = loadCapacity;
            Type = VehicleType.Truck;
        }

        public Truck() : base() { } // EntityFramework ctor
    }
}
