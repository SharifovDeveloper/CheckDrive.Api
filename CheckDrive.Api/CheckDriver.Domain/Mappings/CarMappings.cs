using AutoMapper;
using CheckDrive.Domain.DTOs.Car;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class CarMappings : Profile
    {
        public CarMappings() 
        {
            CreateMap<CarDto, Car>();
            CreateMap<Car, CarDto>();
            CreateMap<CarForCreateDto, Car>();
            CreateMap<CarForUpdateDto, Car>();
        }
    }
}
