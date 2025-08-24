namespace CarAuctionApi.Domain.Entities
{
    public abstract class Vehicle
    {
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }
        public VehicleType Type { get; protected set; }

        protected Vehicle(string id, string manufacturer, string model, int year, decimal startingBid)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            StartingBid = startingBid;
        }

        protected Vehicle() { } // EntityFramework ctor
    }
}
