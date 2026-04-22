using StudySpace.API.DTOs.Booking;

namespace StudySpace.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponseDto>> GetAllBookingsAsync(string? status);
        Task<List<BookingResponseDto>> GetMyBookingsAsync(int userId);
        Task<BookingResponseDto> GetBookingByIdAsync(int id);
        Task<BookingResponseDto> CreateBookingAsync(int userId, BookingRequestDto dto);
        Task<BookingResponseDto> ConfirmBookingAsync(int id);
        Task<BookingResponseDto> CancelBookingAsync(int id);
    }
}