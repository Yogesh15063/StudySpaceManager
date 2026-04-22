using StudySpace.API.DTOs.User;

namespace StudySpace.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<UserResponseDto> UpdateUserStatusAsync(int id, string status);
    }
}