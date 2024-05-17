using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Car;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface ICarService
    {
        Task<GetBaseResponse<CarDto>> GetCarsAsync(CarResourceParameters resourceParameters);
        Task<CarDto?> GetCarByIdAsync(int id);
        Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate);
        Task<CarDto> UpdateCarAsync(CarForUpdateDto carForUpdate);
        Task DeleteCarAsync(int id);
    }
}
