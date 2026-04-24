using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySpace.API.DTOs.Seat;
using StudySpace.API.Helpers;
using StudySpace.API.Services.Interfaces;

namespace StudySpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _seatService.GetAllSeatsAsync();
            return Ok(ApiResponse<List<SeatResponseDto>>.Ok(result));
        }

        [HttpGet("available")]
        [Authorize]
        public async Task<IActionResult> GetAvailable([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _seatService.GetAvailableSeatsAsync(month, year);
            return Ok(ApiResponse<List<SeatResponseDto>>.Ok(result));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _seatService.GetSeatByIdAsync(id);
            return Ok(ApiResponse<SeatResponseDto>.Ok(result));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Add([FromBody] SeatRequestDto dto)
        {
            var result = await _seatService.AddSeatAsync(dto);
            return Ok(ApiResponse<SeatResponseDto>.Ok(result, "Seat added successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] SeatRequestDto dto)
        {
            var result = await _seatService.UpdateSeatAsync(id, dto);
            return Ok(ApiResponse<SeatResponseDto>.Ok(result, "Seat updated successfully"));
        }

        [HttpPatch("{id}/toggle")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Toggle(int id)
        {
            await _seatService.ToggleSeatStatusAsync(id);
            return Ok(ApiResponse<object>.Ok(null!, "Seat status toggled"));
        }

       [HttpGet("grid")]
[Authorize]
public async Task<IActionResult> GetGrid(
    [FromQuery] int month,
    [FromQuery] int year,
    [FromQuery] string? floor)
{
    var result = await _seatService.GetSeatGridAsync(month, year, floor);
    return Ok(ApiResponse<List<SeatStatusDto>>.Ok(result));
}
    }
}