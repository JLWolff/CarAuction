using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> StartAuction([FromBody] StartAuctionDto startAuctionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var auction = await _auctionService.StartAuctionAsync(startAuctionDto);
            
            return this.Ok(auction);
        }

        
        [HttpPost("{id}/close")]
        public async Task<ActionResult> CloseAuction(string id)
        {
            await _auctionService.CloseAuctionAsync(id);
            return Ok();
        }
        
        [HttpPost("{id}/bids")]
        public async Task<ActionResult<AuctionDto>> PlaceBid(string id, [FromBody] CreateBidDto createBidDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var auction = await _auctionService.PlaceBidAsync(id, createBidDto);
            return Ok(auction);
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetActiveAuctions()
        {
            var auctions = await _auctionService.GetActiveAuctionsAsync();
            return Ok(auctions);
        }

        
        [HttpGet("{id}/bids")]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetAuctionBids(string id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            
            if (auction == null)
            {
                return NotFound($"Auction with ID '{id}' not found");
            }

            return Ok(auction.Bids);
        }
    }
}
