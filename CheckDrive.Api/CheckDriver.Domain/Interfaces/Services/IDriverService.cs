using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverDto>> GetAllDriversAsync();
        Task<GetBaseResponse<DriverDto>> GetDriversAsync(DriverResourceParameters resourceParameters);
        Task<DriverDto?> GetDriverByIdAsync(int id);
        Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate);
        Task<DriverDto> UpdateDriverAsync(DriverForUpdateDto driverForUpdate);
        Task DeleteDriverAsync(int id);
    }
}
