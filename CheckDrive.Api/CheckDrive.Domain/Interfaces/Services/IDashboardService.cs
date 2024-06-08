using CheckDrive.ApiContracts.Dashboard;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboard();
    }
}
