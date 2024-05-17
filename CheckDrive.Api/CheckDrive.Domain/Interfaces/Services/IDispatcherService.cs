using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Dispatcher;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDispatcherService
    {
        Task<GetBaseResponse<DispatcherDto>> GetDispatchersAsync(DispatcherResourceParameters resourceParameters);
        Task<DispatcherDto?> GetDispatcherByIdAsync(int id);
        Task<DispatcherDto> CreateDispatcherAsync(DispatcherForCreateDto dispatcherForCreate);
        Task DeleteDispatcherAsync(int id);
    }
}
