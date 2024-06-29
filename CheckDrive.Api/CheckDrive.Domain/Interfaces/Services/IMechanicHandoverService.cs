using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IMechanicHandoverService
    {
        Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversForMechanicsAsync(MechanicHandoverResourceParameters resourceParameters);
        Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversAsync(MechanicHandoverResourceParameters resourceParameters);
        Task<MechanicHandoverDto?> GetMechanicHandoverByIdAsync(int id);
        Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreateDto);
        Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(MechanicHandoverForUpdateDto handoverForUpdateDto);
        Task DeleteMechanicHandoverAsync(int id);
    }
}
