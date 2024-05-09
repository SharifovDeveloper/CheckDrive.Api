using CheckDrive.Domain.DTOs.MechanicHandover;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IMechanicHandoverService
    {
        Task<IEnumerable<MechanicHandoverDto>> GetAllMechanicHandoversAsync();
        Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversAsync(MechanicHandoverResourceParameters resourceParameters);
        Task<MechanicHandoverDto?> GetMechanicHandoverByIdAsync(int id);
        Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreateDto);
        Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(MechanicHandoverForUpdateDto handoverForUpdateDto);
        Task DeleteMechanicHandoverAsync(int id);
    }
}
