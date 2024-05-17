using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Operator;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IOperatorService
    {
        Task<GetBaseResponse<OperatorDto>> GetOperatorsAsync(OperatorResourceParameters resourceParameters);
        Task<OperatorDto?> GetOperatorByIdAsync(int id);
        Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate);
        Task DeleteOperatorAsync(int id);
    }
}
