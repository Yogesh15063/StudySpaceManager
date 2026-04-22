using Microsoft.EntityFrameworkCore;
using StudySpace.API.Data;
using StudySpace.API.DTOs.Booking;
using StudySpace.API.Models;
using StudySpace.API.Services.Interfaces;

namespace StudySpace.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingResponseDto>> GetAllBookingsAsync(string? status)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Seat)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.Status == status);

            return await query.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = b.User.Name,
                SeatId = b.SeatId,
                SeatNumber = b.Seat.SeatNumber,
                Month = b.Month,
                Year = b.Year,
                Status = b.Status,
                CreatedAt = b.CreatedAt
            }).ToListAsync();
        }

        public async Task<List<BookingResponseDto>> GetMyBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Seat)
                .Where(b => b.UserId == userId)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    UserName = b.User.Name,
                    SeatId = b.SeatId,
                    SeatNumber = b.Seat.SeatNumber,
                    Month = b.Month,
                    Year = b.Year,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
                }).ToListAsync();
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            return new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                UserName = booking.User.Name,
                SeatId = booking.SeatId,
                SeatNumber = booking.Seat.SeatNumber,
                Month = booking.Month,
                Year = booking.Year,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt
            };
        }

        public async Task<BookingResponseDto> CreateBookingAsync(int userId, BookingRequestDto dto)
        {
            // Check if member already has a booking for this month
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId
                    && b.Month == dto.Month
                    && b.Year == dto.Year
                    && b.Status != "Cancelled");

            if (existingBooking != null)
                throw new InvalidOperationException(
                    "You already have a booking for this month");

            // Check if seat is available
            var seatTaken = await _context.Bookings
                .AnyAsync(b => b.SeatId == dto.SeatId
                    && b.Month == dto.Month
                    && b.Year == dto.Year
                    && b.Status != "Cancelled");

            if (seatTaken)
                throw new InvalidOperationException(
                    "This seat is already booked for this month");

            // Check seat exists and is active
            var seat = await _context.Seats.FindAsync(dto.SeatId);
            if (seat == null || !seat.IsActive)
                throw new KeyNotFoundException("Seat not found or inactive");

            var booking = new Booking
            {
                UserId = userId,
                SeatId = dto.SeatId,
                Month = dto.Month,
                Year = dto.Year,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return await GetBookingByIdAsync(booking.Id);
        }

        public async Task<BookingResponseDto> ConfirmBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.Status != "Pending")
                throw new InvalidOperationException(
                    "Only pending bookings can be confirmed");

            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();

            return await GetBookingByIdAsync(id);
        }

        public async Task<BookingResponseDto> CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.Status == "Cancelled")
                throw new InvalidOperationException(
                    "Booking is already cancelled");

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return await GetBookingByIdAsync(id);
        }
    }
}