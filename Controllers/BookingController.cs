using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudySpace.API.DTOs.Booking;
using StudySpace.API.Helpers;
using StudySpace.API.Services.Interfaces;
using System.Security.Claims;

namespace StudySpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var result = await _bookingService.GetAllBookingsAsync(status);
            return Ok(ApiResponse<List<BookingResponseDto>>.Ok(result));
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var result = await _bookingService.GetMyBookingsAsync(userId);
            return Ok(ApiResponse<List<BookingResponseDto>>.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(ApiResponse<BookingResponseDto>.Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var result = await _bookingService.CreateBookingAsync(userId, dto);
            return Ok(ApiResponse<BookingResponseDto>.Ok(result, 
                "Booking request submitted successfully"));
        }

        [HttpPatch("{id}/confirm")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _bookingService.ConfirmBookingAsync(id);
            return Ok(ApiResponse<BookingResponseDto>.Ok(result, 
                "Booking confirmed successfully"));
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id);
            return Ok(ApiResponse<BookingResponseDto>.Ok(result, 
                "Booking cancelled successfully"));
        }
    }
}