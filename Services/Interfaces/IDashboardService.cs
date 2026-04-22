using StudySpace.API.DTOs;

namespace StudySpace.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetStatsAsync();
    }
}