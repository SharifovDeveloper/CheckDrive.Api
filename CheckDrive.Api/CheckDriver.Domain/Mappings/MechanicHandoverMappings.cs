using AutoMapper;
using CheckDrive.Domain.DTOs.MechanicHandover;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicHandoverMappings : Profile
    {
        public MechanicHandoverMappings() 
        {
            CreateMap<MechanicHandoverDto, MechanicHandover>();
            CreateMap<MechanicHandover, MechanicHandoverDto>();
            CreateMap<MechanicHandoverForCreateDto, MechanicHandover>();
            CreateMap<MechanicHandoverForUpdateDto, MechanicHandover>();
        }
    }
}