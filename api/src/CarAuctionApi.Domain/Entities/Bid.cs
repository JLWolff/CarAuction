namespace CarAuctionApi.Domain.Entities
{
    public class Bid
    {
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string BidderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public Bid(string id, string auctionId, string bidderId, decimal amount)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            AuctionId = auctionId ?? throw new ArgumentNullException(nameof(auctionId));
            BidderId = bidderId ?? throw new ArgumentNullException(nameof(bidderId));
            Amount = amount;
            Timestamp = DateTime.UtcNow;
        }

        public Bid() { } // EntityFramework ctor
    }
}
