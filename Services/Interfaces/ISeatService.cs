using StudySpace.API.DTOs.Seat;

namespace StudySpace.API.Services.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatResponseDto>> GetAllSeatsAsync();
        Task<List<SeatResponseDto>> GetAvailableSeatsAsync(int month, int year);
        Task<SeatResponseDto> GetSeatByIdAsync(int id);
        Task<SeatResponseDto> AddSeatAsync(SeatRequestDto dto);
        Task<SeatResponseDto> UpdateSeatAsync(int id, SeatRequestDto dto);
        Task ToggleSeatStatusAsync(int id);
    }
}