using CheckDrive.Domain.DTOs.DispatcherReview;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDispatcherReviewService
    {
        Task<IEnumerable<DispatcherReviewDto>> GetAllDispatcherReviewsAsync();
        Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsAsync(DispatcherReviewResourceParameters resourceParameters);
        Task<DispatcherReviewDto?> GetDispatcherReviewByIdAsync(int id);
        Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReviewForCreate);
        Task<DispatcherReviewDto> UpdateDispatcherReviewAsync(DispatcherReviewForUpdateDto dispatcherReviewForUpdate);
        Task DeleteDispatcherReviewAsync(int id);
    }
}
