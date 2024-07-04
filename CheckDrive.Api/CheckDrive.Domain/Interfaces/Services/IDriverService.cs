using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Driver;
using System.Data;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDriverService
    {
        Task<GetBaseResponse<DriverDto>> GetDriversAsync(DriverResourceParameters resourceParameters);
        Task<DriverDto?> GetDriverByIdAsync(int id);
        Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate);
        Task DeleteDriverAsync(int id);
        Task<IEnumerable<DriverHistoryDto>> GetDriverHistories(int Id);
    }
}
