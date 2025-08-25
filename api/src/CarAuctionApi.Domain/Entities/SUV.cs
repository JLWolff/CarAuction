namespace CarAuctionApi.Domain.Entities
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; set; }

        public SUV(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfSeats = numberOfSeats;
            Type = VehicleType.SUV;
        }

        public SUV() : base() { } // EntityFramework ctor
    }
}
