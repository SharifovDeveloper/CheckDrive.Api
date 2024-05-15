using CheckDrive.Domain.DTOs.Account;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDriverService
    {
        Task<GetBaseResponse<DriverDto>> GetDriversAsync(DriverResourceParameters resourceParameters);
        Task<DriverDto?> GetDriverByIdAsync(int id);
        Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate);
        Task DeleteDriverAsync(int id);
    }
}
