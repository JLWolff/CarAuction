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
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty.", nameof(id));
            }

            if (string.IsNullOrEmpty(manufacturer))
            {
                throw new ArgumentException("Manufacturer cannot be null or empty.", nameof(manufacturer));
            }

            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentException("Model cannot be null or empty.", nameof(model));
            }
            
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            StartingBid = startingBid;
        }

        protected Vehicle() { } // EntityFramework ctor
    }
}
