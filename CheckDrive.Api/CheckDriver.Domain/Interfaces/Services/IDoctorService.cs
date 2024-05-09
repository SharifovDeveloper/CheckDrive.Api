using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<GetBaseResponse<DoctorDto>> GetDoctorsAsync(DoctorResourceParameters resourceParameters);
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate);
        Task<DoctorDto> UpdateDoctorAsync(DoctorForUpdateDto doctorForUpdate);
        Task DeleteDoctorAsync(int id);
    }
}
