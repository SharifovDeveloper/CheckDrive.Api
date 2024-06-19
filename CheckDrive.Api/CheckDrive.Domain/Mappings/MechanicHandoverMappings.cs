using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicHandoverMappings : Profile
    {
        public MechanicHandoverMappings()
        {
            CreateMap<MechanicHandoverDto, MechanicHandover>();
            CreateMap<MechanicHandover, MechanicHandoverDto>()
                .ForMember(d => d.CarName, f => f.MapFrom(e => e.Car.Model))
                .ForMember(d => d.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(d => d.MechanicName, f => f.MapFrom(e => $"{e.Mechanic.Account.FirstName} {e.Mechanic.Account.LastName}"))
                .ForMember(x => x.AccountDriverId, f => f.MapFrom(e => e.Driver.Account.Id));
            CreateMap<MechanicHandoverForCreateDto, MechanicHandover>();
            CreateMap<MechanicHandoverForUpdateDto, MechanicHandover>();
        }
    }
}