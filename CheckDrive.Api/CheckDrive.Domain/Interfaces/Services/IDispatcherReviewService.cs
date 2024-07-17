using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDispatcherReviewService
    {
        Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsForDispatcherAsync(DispatcherReviewResourceParameters resourceParameters);
        Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsAsync(DispatcherReviewResourceParameters resourceParameters);
        Task<DispatcherReviewDto?> GetDispatcherReviewByIdAsync(int id);
        Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReviewForCreate);
        Task<DispatcherReviewDto> UpdateDispatcherReviewAsync(DispatcherReviewForUpdateDto dispatcherReviewForUpdate);
        Task DeleteDispatcherReviewAsync(int id);
        Task<IEnumerable<DispatcherReviewDto>> GetDispatcherHistories(int? Id);
    }
}
