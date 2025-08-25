using CarAuctionApi.Domain.Entities;
using Xunit;

namespace CarAuctionApi.Tests.Domain
{
    public class AuctionTests
    {
        [Fact]
        public void Constructor_SetsStatusToDraft()
        {
            var a = MakeDraftAuction();
            Assert.Equal(AuctionStatus.Draft, a.Status);
        }

        [Fact]
        public void Constructor_SetsCurrentHighestBidToVehicleStartingBid()
        {
            var v = MakeVehicle(startingBid: 3210m);
            var a = new Auction(v.Id, v);
            Assert.Equal(3210m, a.CurrentHighestBid);
        }

        [Fact]
        public void Constructor_SetsVehicleReference()
        {
            var v = MakeVehicle();
            var a = new Auction(v.Id, v);
            Assert.Same(v, a.Vehicle);
        }

        [Fact]
        public void Constructor_SetsVehicleId()
        {
            var v = MakeVehicle(id: "v1");
            var a = new Auction(v.Id, v);
            Assert.Equal("v1", a.VehicleId);
        }

        [Fact]
        public void Constructor_GeneratesAuctionId()
        {
            var a = MakeDraftAuction();
            Assert.True(Guid.TryParse(a.Id, out _));
        }

        [Fact]
        public void Constructor_InitializesBidsEmpty()
        {
            var a = MakeDraftAuction();
            Assert.Empty(a.Bids);
        }

        [Fact]
        public void Start_FromDraft_SetsStatusToActive()
        {
            var a = MakeDraftAuction();
            a.Start();
            Assert.Equal(AuctionStatus.Active, a.Status);
        }

        [Fact]
        public void Start_FromDraft_SetsStartTime()
        {
            var a = MakeDraftAuction();
            a.Start();
            Assert.NotEqual(default, a.StartTime);
        }

        [Fact]
        public void Start_WhenAlreadyActive_Throws()
        {
            var a = MakeActiveAuction();
            Assert.Throws<InvalidOperationException>(() => a.Start());
        }

        [Fact]
        public void PlaceBid_WhenDraft_Throws()
        {
            var a = MakeDraftAuction();
            var bidderId = Guid.NewGuid().ToString();
            Assert.Throws<InvalidOperationException>(() => a.PlaceBid(bidderId, a.CurrentHighestBid + 1));
        }

        [Fact]
        public void PlaceBid_WhenClosed_Throws()
        {
            var a = MakeActiveAuction();
            a.Close();
            var bidderId = Guid.NewGuid().ToString();
            Assert.Throws<InvalidOperationException>(() => a.PlaceBid(bidderId, a.CurrentHighestBid + 1));
        }

        [Fact]
        public void PlaceBid_WithEmptyBidder_Throws()
        {
            var a = MakeActiveAuction();
            Assert.Throws<ArgumentException>(() => a.PlaceBid("", a.CurrentHighestBid + 1));
        }

        [Fact]
        public void PlaceBid_WithEqualAmount_Throws()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            Assert.Throws<ArgumentException>(() => a.PlaceBid(bidderId, a.CurrentHighestBid));
        }

        [Fact]
        public void PlaceBid_WithLowerAmount_Throws()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            Assert.Throws<ArgumentException>(() => a.PlaceBid(bidderId, a.CurrentHighestBid - 1));
        }

        [Fact]
        public void PlaceBid_Valid_AddsOneBid()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            a.PlaceBid(bidderId, a.CurrentHighestBid + 100m);
            Assert.Single(a.Bids);
        }

        [Fact]
        public void PlaceBid_Valid_UpdatesCurrentHighestBid()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            var newAmount = a.CurrentHighestBid + 150m;
            a.PlaceBid(bidderId, newAmount);
            Assert.Equal(newAmount, a.CurrentHighestBid);
        }

        [Fact]
        public void PlaceBid_Valid_UpdatesHighestBidderId()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            a.PlaceBid(bidderId, a.CurrentHighestBid + 50m);
            Assert.Equal(bidderId, a.HighestBidderId);
        }

        [Fact]
        public void PlaceBid_Valid_SetsBidAuctionId()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            a.PlaceBid(bidderId, a.CurrentHighestBid + 10m);
            var last = a.Bids.Last();
            Assert.Equal(a.Id, last.AuctionId);
        }

        [Fact]
        public void PlaceBid_Valid_SetsBidAmount()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            var newAmount = a.CurrentHighestBid + 25m;
            a.PlaceBid(bidderId, newAmount);
            var last = a.Bids.Last();
            Assert.Equal(newAmount, last.Amount);
        }

        [Fact]
        public void PlaceBid_Valid_SetsBidderIdOnBid()
        {
            var a = MakeActiveAuction();
            var bidderId = Guid.NewGuid().ToString();
            a.PlaceBid(bidderId, a.CurrentHighestBid + 75m);
            var last = a.Bids.Last();
            Assert.Equal(bidderId, last.BidderId);
        }

        [Fact]
        public void PlaceBid_SecondHigherBid_IncreasesBidCountToTwo()
        {
            var a = MakeActiveAuction();
            var bidder1 = Guid.NewGuid().ToString();
            var bidder2 = Guid.NewGuid().ToString();

            a.PlaceBid(bidder1, a.CurrentHighestBid + 100m);
            a.PlaceBid(bidder2, a.CurrentHighestBid + 100m);

            Assert.Equal(2, a.Bids.Count);
        }

        [Fact]
        public void PlaceBid_SecondHigherBid_SetsHighestBidderToSecond()
        {
            var a = MakeActiveAuction();
            var bidder1 = Guid.NewGuid().ToString();
            var bidder2 = Guid.NewGuid().ToString();

            a.PlaceBid(bidder1, a.CurrentHighestBid + 10m);
            var next = a.CurrentHighestBid + 10m;
            a.PlaceBid(bidder2, next);

            Assert.Equal(bidder2, a.HighestBidderId);
        }

        [Fact]
        public void PlaceBid_SecondHigherBid_UpdatesCurrentHighestBid()
        {
            var a = MakeActiveAuction();
            var bidder1 = Guid.NewGuid().ToString();
            var bidder2 = Guid.NewGuid().ToString();

            a.PlaceBid(bidder1, a.CurrentHighestBid + 5m);
            var next = a.CurrentHighestBid + 25m;
            a.PlaceBid(bidder2, next);

            Assert.Equal(next, a.CurrentHighestBid);
        }

        [Fact]
        public void Close_FromActive_SetsStatusToClosed()
        {
            var a = MakeActiveAuction();
            a.Close();
            Assert.Equal(AuctionStatus.Closed, a.Status);
        }

        [Fact]
        public void Close_FromActive_SetsEndTime()
        {
            var a = MakeActiveAuction();
            a.Close();
            Assert.NotNull(a.EndTime);
        }

        [Fact]
        public void Close_FromDraft_Throws()
        {
            var a = MakeDraftAuction();
            Assert.Throws<InvalidOperationException>(() => a.Close());
        }

        [Fact]
        public void Close_WhenAlreadyClosed_Throws()
        {
            var a = MakeActiveAuction();
            a.Close();
            Assert.Throws<InvalidOperationException>(() => a.Close());
        }

        [Fact]
        public void PlaceBid_AfterClose_Throws()
        {
            var a = MakeActiveAuction();
            a.Close();
            var bidderId = Guid.NewGuid().ToString();
            Assert.Throws<InvalidOperationException>(() => a.PlaceBid(bidderId, a.CurrentHighestBid + 1));
        }
        
        private static Vehicle MakeVehicle(
            string id = "v1",
            decimal startingBid = 5000m)
        {
            return new Sedan(id, "manufacturer", "model", 2020, startingBid, 4);
        }

        private static Auction MakeDraftAuction(string vehicleId = "v1")
        {
            var v = MakeVehicle(id: vehicleId);
            return new Auction(vehicleId, v);
        }

        private static Auction MakeActiveAuction()
        {
            var a = MakeDraftAuction();
            a.Start();
            return a;
        }
    }
}
