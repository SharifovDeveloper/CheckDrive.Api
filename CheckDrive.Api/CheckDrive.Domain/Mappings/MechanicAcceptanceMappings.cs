using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.MechanicAcceptance;

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
