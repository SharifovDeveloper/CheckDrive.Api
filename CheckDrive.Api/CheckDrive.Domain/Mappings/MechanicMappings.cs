using AutoMapper;
using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicMappings : Profile
    {
        public MechanicMappings()
        {
            CreateMap<Mechanic, MechanicDto>()
                .ForMember(x => x.FirstName, e => e.MapFrom(f => f.Account.FirstName))
                .ForMember(x => x.LastName, e => e.MapFrom(f => f.Account.LastName))
                .ForMember(x => x.Login, e => e.MapFrom(f => f.Account.Login))
                .ForMember(x => x.Password, e => e.MapFrom(f => f.Account.Password))
                .ForMember(x => x.Birthdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<MechanicForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 6));
            CreateMap<Account, MechanicDto>();
            CreateMap<AccountForUpdateDto, Mechanic>();
        }
    }
}
