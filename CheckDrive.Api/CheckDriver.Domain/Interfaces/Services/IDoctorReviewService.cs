﻿using CheckDrive.Domain.DTOs.DoctorReview;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDoctorReviewService
    {
        Task<IEnumerable<DoctorReviewDto>> GetAllDoctorReviewsAsync();
        Task<GetBaseResponse<DoctorReviewDto>> GetDoctorReviewsAsync(DoctorReviewResourceParameters resourceParameters);
        Task<DoctorReviewDto?> GetDoctorReviewByIdAsync(int id);
        Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto doctorReviewForCreate);
        Task<DoctorReviewDto> UpdateDoctorReviewAsync(DoctorReviewForUpdateDto doctorReviewForUpdate);
        Task DeleteDoctorReviewAsync(int id);
    }
}
