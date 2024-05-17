using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Doctor;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<GetBaseResponse<DoctorDto>> GetDoctorsAsync(DoctorResourceParameters resourceParameters);
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate);
        Task DeleteDoctorAsync(int id);
    }
}
