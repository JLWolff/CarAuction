using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Domain.Entities;

namespace CarAuctionApi.Application.Extensions;

public static class AuctionExtensions
{
    public static AuctionDto MapToDto(this Auction @this)
    {
        return new AuctionDto
        {
            Id = @this.Id,
            VehicleId = @this.VehicleId,
            Vehicle = @this.Vehicle.ToDto(),
            Status = @this.Status,
            CurrentHighestBid = @this.CurrentHighestBid,
            HighestBidderId = @this.HighestBidderId,
            StartTime = @this.StartTime,
            EndTime = @this.EndTime,
            Bids = @this.Bids.Select(bid => bid.ToDto()).ToList()
        };
    }
}