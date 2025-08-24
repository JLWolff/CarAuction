namespace CarAuctionApi.Domain.Entities
{
    public class Auction
    {
        public string Id { get; private set; }
        public string VehicleId { get; private set; }
        public Vehicle Vehicle { get; private set; }

        public AuctionStatus Status { get; private set; }

        public decimal CurrentHighestBid { get; private set; }
        public string? HighestBidderId { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        public List<Bid> Bids { get; private set; }

        // EntityFramework ctor
        public Auction()
        {
            Bids = new List<Bid>();
        }

        public Auction(string vehicleId, Vehicle vehicle)
        {
            Id = Guid.NewGuid().ToString();
            VehicleId = vehicleId ?? throw new ArgumentNullException(nameof(vehicleId));
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));

            Status = AuctionStatus.Draft;
            CurrentHighestBid = vehicle.StartingBid;
            Bids = new List<Bid>();
        }

        public void Start()
        {
            if (Status == AuctionStatus.Active)
                throw new InvalidOperationException("Auction is already active.");

            if (Status == AuctionStatus.Closed)
                throw new InvalidOperationException("Cannot start a closed auction.");

            Status = AuctionStatus.Active;
            StartTime = DateTime.UtcNow;
            // CurrentHighestBid already equals Vehicle.StartingBid; keep as-is.
        }

        public void PlaceBid(string bidderId, decimal amount)
        {
            if (Status != AuctionStatus.Active)
                throw new InvalidOperationException("Cannot place bid on inactive auction.");

            if (string.IsNullOrWhiteSpace(bidderId))
                throw new ArgumentException("Bidder id is required.", nameof(bidderId));

            if (amount <= CurrentHighestBid)
                throw new ArgumentException("Bid amount must be greater than current highest bid.", nameof(amount));

            var bid = new Bid(Guid.NewGuid().ToString(), Id, bidderId, amount);
            Bids.Add(bid);

            CurrentHighestBid = amount;
            HighestBidderId = bidderId;
        }

        public void Close()
        {
            if (Status != AuctionStatus.Active)
                throw new InvalidOperationException("Cannot close inactive auction.");

            Status = AuctionStatus.Closed;
            EndTime = DateTime.UtcNow;
        }
    }
}
