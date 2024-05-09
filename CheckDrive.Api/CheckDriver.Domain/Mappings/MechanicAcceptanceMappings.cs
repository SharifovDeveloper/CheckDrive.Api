using AutoMapper;
using CheckDrive.Domain.DTOs.MechanicAcceptance;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicAcceptanceMappings : Profile
    {
        public MechanicAcceptanceMappings() 
        {
            CreateMap<MechanicAcceptanceDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptance, MechanicAcceptanceDto>();
            CreateMap<MechanicAcceptanceForCreateDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptanceForUpdateDto, MechanicAcceptance>();
        }
    }
}
