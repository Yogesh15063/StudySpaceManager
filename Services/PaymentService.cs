using Microsoft.EntityFrameworkCore;
using StudySpace.API.Data;
using StudySpace.API.DTOs.Payment;
using StudySpace.API.Models;
using StudySpace.API.Services.Interfaces;

namespace StudySpace.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Seat)
                .Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    BookingId = p.BookingId,
                    SeatNumber = p.Booking.Seat.SeatNumber,
                    Amount = p.Amount,
                    Method = p.Method,
                    Status = p.Status,
                    PaidOn = p.PaidOn
                }).ToListAsync();
        }

        public async Task<List<PaymentResponseDto>> GetMyPaymentsAsync(int userId)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Seat)
                .Where(p => p.UserId == userId)
                .Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    BookingId = p.BookingId,
                    SeatNumber = p.Booking.Seat.SeatNumber,
                    Amount = p.Amount,
                    Method = p.Method,
                    Status = p.Status,
                    PaidOn = p.PaidOn
                }).ToListAsync();
        }

        public async Task<List<PaymentResponseDto>> GetOverduePaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Seat)
                .Where(p => p.Status == "Unpaid")
                .Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    BookingId = p.BookingId,
                    SeatNumber = p.Booking.Seat.SeatNumber,
                    Amount = p.Amount,
                    Method = p.Method,
                    Status = p.Status,
                    PaidOn = p.PaidOn
                }).ToListAsync();
        }

        public async Task<PaymentResponseDto> RecordPaymentAsync(PaymentRequestDto dto)
        {
            // Check booking exists
            var booking = await _context.Bookings.FindAsync(dto.BookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            // Check payment not already recorded
            var existing = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == dto.BookingId
                    && p.Status == "Paid");
            if (existing != null)
                throw new InvalidOperationException(
                    "Payment already recorded for this booking");

            var payment = new Payment
            {
                UserId = dto.UserId,
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                Method = dto.Method,
                Status = "Paid",
                PaidOn = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            // Update member status to Active
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user != null)
                user.Status = "Active";

            await _context.SaveChangesAsync();

            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Seat)
                .Where(p => p.Id == payment.Id)
                .Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User.Name,
                    BookingId = p.BookingId,
                    SeatNumber = p.Booking.Seat.SeatNumber,
                    Amount = p.Amount,
                    Method = p.Method,
                    Status = p.Status,
                    PaidOn = p.PaidOn
                }).FirstAsync();
        }
    }
}