using Microsoft.EntityFrameworkCore;
using StudySpace.API.Data;
using StudySpace.API.DTOs;
using StudySpace.API.Services.Interfaces;

namespace StudySpace.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetStatsAsync()
        {
            var now = DateTime.UtcNow;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var totalSeats = await _context.Seats
                .CountAsync(s => s.IsActive);

            var occupiedSeats = await _context.Bookings
                .CountAsync(b => b.Month == currentMonth
                    && b.Year == currentYear
                    && b.Status == "Confirmed");

            var pendingBookings = await _context.Bookings
                .CountAsync(b => b.Status == "Pending");

            var overduePayments = await _context.Payments
                .CountAsync(p => p.Status == "Unpaid");

            var revenueThisMonth = await _context.Payments
                .Where(p => p.PaidOn.HasValue
                    && p.PaidOn.Value.Month == currentMonth
                    && p.PaidOn.Value.Year == currentYear
                    && p.Status == "Paid")
                .SumAsync(p => p.Amount);

            var renewalsThisWeek = await _context.Bookings
                .CountAsync(b => b.Month == currentMonth
                    && b.Year == currentYear
                    && b.Status == "Confirmed"
                    && now.Day >= 25);

            return new DashboardResponseDto
            {
                TotalSeats = totalSeats,
                OccupiedSeats = occupiedSeats,
                AvailableSeats = totalSeats - occupiedSeats,
                PendingBookings = pendingBookings,
                OverduePayments = overduePayments,
                RevenueThisMonth = revenueThisMonth,
                RenewalsThisWeek = renewalsThisWeek
            };
        }
    }
}