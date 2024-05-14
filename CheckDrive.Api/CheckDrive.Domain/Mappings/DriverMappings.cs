using AutoMapper;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DriverMappings : Profile
    {
        public DriverMappings() 
        {
            CreateMap<DriverDto, Driver>();
            CreateMap<Driver, DriverDto>();
            CreateMap<DriverForCreateDto, Driver>();
            CreateMap<DriverForUpdateDto, Driver>();
        }
    }
}
