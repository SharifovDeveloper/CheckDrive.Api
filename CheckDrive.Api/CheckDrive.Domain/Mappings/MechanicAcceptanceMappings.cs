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
            CreateMap<MechanicAcceptance, MechanicAcceptanceDto>()
                .ForMember(d => d.MechanicHandoverName, f => f.MapFrom(e => $"{e.MechanicHandover.Mechanic.Account.FirstName} {e.MechanicHandover.Mechanic.Account.LastName}"));
            CreateMap<MechanicAcceptanceForCreateDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptanceForUpdateDto, MechanicAcceptance>();
        }
    }
}
