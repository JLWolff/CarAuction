using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.Extensions;

public static class BidExtensions
{
    public static BidDto ToDto(this Bid @this)
    {
        return new BidDto
        {
            Id = @this.Id,
            AuctionId = @this.AuctionId,
            BidderId = @this.BidderId,
            Amount = @this.Amount,
            Timestamp = @this.Timestamp
        };
    }
}