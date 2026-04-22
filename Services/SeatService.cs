using Microsoft.EntityFrameworkCore;
using StudySpace.API.Data;
using StudySpace.API.DTOs.Seat;
using StudySpace.API.Models;
using StudySpace.API.Services.Interfaces;

namespace StudySpace.API.Services
{
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;

        public SeatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeatResponseDto>> GetAllSeatsAsync()
        {
            return await _context.Seats
                .Select(s => new SeatResponseDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Floor = s.Floor,
                    Type = s.Type,
                    MonthlyPrice = s.MonthlyPrice,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }

        public async Task<List<SeatResponseDto>> GetAvailableSeatsAsync(int month, int year)
        {
            var bookedSeatIds = await _context.Bookings
                .Where(b => b.Month == month
                    && b.Year == year
                    && b.Status != "Cancelled")
                .Select(b => b.SeatId)
                .ToListAsync();

            return await _context.Seats
                .Where(s => s.IsActive && !bookedSeatIds.Contains(s.Id))
                .Select(s => new SeatResponseDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Floor = s.Floor,
                    Type = s.Type,
                    MonthlyPrice = s.MonthlyPrice,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }

        public async Task<SeatResponseDto> GetSeatByIdAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found");

            return new SeatResponseDto
            {
                Id = seat.Id,
                SeatNumber = seat.SeatNumber,
                Floor = seat.Floor,
                Type = seat.Type,
                MonthlyPrice = seat.MonthlyPrice,
                IsActive = seat.IsActive
            };
        }

        public async Task<SeatResponseDto> AddSeatAsync(SeatRequestDto dto)
        {
            var seat = new Seat
            {
                SeatNumber = dto.SeatNumber,
                Floor = dto.Floor,
                Type = dto.Type,
                MonthlyPrice = dto.MonthlyPrice,
                IsActive = true
            };

            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            return new SeatResponseDto
            {
                Id = seat.Id,
                SeatNumber = seat.SeatNumber,
                Floor = seat.Floor,
                Type = seat.Type,
                MonthlyPrice = seat.MonthlyPrice,
                IsActive = seat.IsActive
            };
        }

        public async Task<SeatResponseDto> UpdateSeatAsync(int id, SeatRequestDto dto)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found");

            seat.SeatNumber = dto.SeatNumber;
            seat.Floor = dto.Floor;
            seat.Type = dto.Type;
            seat.MonthlyPrice = dto.MonthlyPrice;

            await _context.SaveChangesAsync();

            return new SeatResponseDto
            {
                Id = seat.Id,
                SeatNumber = seat.SeatNumber,
                Floor = seat.Floor,
                Type = seat.Type,
                MonthlyPrice = seat.MonthlyPrice,
                IsActive = seat.IsActive
            };
        }

        public async Task ToggleSeatStatusAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                throw new KeyNotFoundException("Seat not found");

            seat.IsActive = !seat.IsActive;
            await _context.SaveChangesAsync();
        }
    }
}