using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.DoctorReview;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDoctorReviewService
    {
        Task<GetBaseResponse<DoctorReviewDto>> GetDoctorReviewsAsync(DoctorReviewResourceParameters resourceParameters);
        Task<DoctorReviewDto?> GetDoctorReviewByIdAsync(int id);
        Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto doctorReviewForCreate);
        Task<DoctorReviewDto> UpdateDoctorReviewAsync(DoctorReviewForUpdateDto doctorReviewForUpdate);
        Task DeleteDoctorReviewAsync(int id);
    }
}
