using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.OperatorReview;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IOperatorReviewService
    {
        Task<GetBaseResponse<OperatorReviewDto>> GetOperatorReviewsForOperatorAsync(OperatorReviewResourceParameters resourceParameters);
        Task<GetBaseResponse<OperatorReviewDto>> GetOperatorReviewsAsync(OperatorReviewResourceParameters resourceParameters);
        Task<OperatorReviewDto?> GetOperatorReviewByIdAsync(int id);
        Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto reviewForCreateDto);
        Task<OperatorReviewDto> UpdateOperatorReviewAsync(OperatorReviewForUpdateDto reviewForUpdateDto);
        Task DeleteOperatorReviewAsync(int id);
    }
}
