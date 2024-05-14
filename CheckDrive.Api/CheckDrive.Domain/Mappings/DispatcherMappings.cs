using AutoMapper;
using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DispatcherMappings : Profile
    {
        public DispatcherMappings() 
        {
            CreateMap<DispatcherDto, Dispatcher>();
            CreateMap<Dispatcher, DispatcherDto>();
            CreateMap<DispatcherForCreateDto, Dispatcher>();
            CreateMap<DispatcherForUpdateDto, Dispatcher>();
        }
    }
}
