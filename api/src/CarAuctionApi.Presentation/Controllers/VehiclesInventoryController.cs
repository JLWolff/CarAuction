using CarAuctionApi.Application.DTOs;
using CarAuctionApi.Application.Interfaces;
using CarAuctionApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/vehiclesInventory")]
    public class VehiclesInventoryController : ControllerBase
    {
        private readonly IVehiclesInventoryService _vehiclesInventoryService;

        public VehiclesInventoryController(IVehiclesInventoryService vehiclesInventoryService)
        {
            _vehiclesInventoryService = vehiclesInventoryService;
        }
        
        [HttpPost]
        public async Task<ActionResult<VehicleDto>> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _vehiclesInventoryService.AddVehicleAsync(vehicleDto);
            
            return this.StatusCode(StatusCodes.Status201Created, vehicle);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> SearchVehicles(
            [FromQuery] VehicleType? type,
            [FromQuery] string? manufacturer,
            [FromQuery] string? model,
            [FromQuery] int? year)
        {
            var vehicles = await _vehiclesInventoryService.SearchVehiclesAsync(type, manufacturer, model, year);
            return Ok(vehicles);
        }
    }
}
