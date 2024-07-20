using AutoMapper;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicAcceptanceMappings : Profile
    {
        public MechanicAcceptanceMappings()
        {
            CreateMap<MechanicAcceptanceDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptance, MechanicAcceptanceDto>()
                .ForMember(d => d.CarName, f => f.MapFrom(e => $"{e.Car.Model} ({e.Car.Number})"))
                .ForMember(d => d.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(d => d.MechanicName, f => f.MapFrom(e => $"{e.Mechanic.Account.FirstName} {e.Mechanic.Account.LastName}"))
                .ForMember(x => x.AccountDriverId, f => f.MapFrom(e => e.Driver.Account.Id));
            CreateMap<MechanicAcceptanceForCreateDto, MechanicAcceptance>();
            CreateMap<MechanicAcceptanceForUpdateDto, MechanicAcceptance>();
        }
    }
}
