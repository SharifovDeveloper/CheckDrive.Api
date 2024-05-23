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
                .ForMember(d => d.CarName, f => f.MapFrom(e => e.Car.Model))
                .ForMember(d => d.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(d => d.MechanicName, f => f.MapFrom(e => $"{e.Mechanic.Account.FirstName} {e.Mechanic.Account.LastName}"));
            CreateMap<MechanicAcceptanceForCreateDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptanceForUpdateDto, MechanicAcceptance>();
        }
    }
}
