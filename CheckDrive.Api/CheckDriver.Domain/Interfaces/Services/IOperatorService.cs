using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync();
        Task<GetBaseResponse<OperatorDto>> GetOperatorsAsync(OperatorResourceParameters resourceParameters);
        Task<OperatorDto?> GetOperatorByIdAsync(int id);
        Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate);
        Task<OperatorDto> UpdateOperatorAsync(OperatorForUpdateDto operatorForUpdate);
        Task DeleteOperatorAsync(int id);
    }
}
