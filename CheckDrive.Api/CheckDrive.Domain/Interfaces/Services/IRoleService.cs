using CheckDrive.ApiContracts.Role;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IRoleService
    {
        Task<GetBaseResponse<RoleDto>> GetRolesAsync(RoleResourceParameters resourceParameters);
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(RoleForCreateDto roleForCreate);
        Task<RoleDto> UpdateRoleAsync(RoleForUpdateDto roleForUpdate);
        Task DeleteRoleAsync(int id);
    }
}
