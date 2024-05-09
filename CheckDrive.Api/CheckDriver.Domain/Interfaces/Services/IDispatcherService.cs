using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDispatcherService
    {
        Task<IEnumerable<DispatcherDto>> GetAllDispatchersAsync();
        Task<GetBaseResponse<DispatcherDto>> GetDispatchersAsync(DispatcherResourceParameters resourceParameters);
        Task<DispatcherDto?> GetDispatcherByIdAsync(int id);
        Task<DispatcherDto> CreateDispatcherAsync(DispatcherForCreateDto dispatcherForCreate);
        Task<DispatcherDto> UpdateDispatcherAsync(DispatcherForUpdateDto dispatcherForUpdate);
        Task DeleteDispatcherAsync(int id);
    }
}
