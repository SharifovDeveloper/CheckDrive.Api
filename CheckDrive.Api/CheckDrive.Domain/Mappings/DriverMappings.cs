using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.ApiContracts.Account;



namespace CheckDrive.Domain.Mappings
{
    public class DriverMappings : Profile
    {
        public DriverMappings()
        {
            CreateMap<Driver, DriverDto>()
                .ForMember(x => x.FirstName, e => e.MapFrom(f => f.Account.FirstName))
                .ForMember(x => x.LastName, e => e.MapFrom(f => f.Account.LastName))
                .ForMember(x => x.Login, e => e.MapFrom(f => f.Account.Login))
                .ForMember(x => x.Password, e => e.MapFrom(f => f.Account.Password))
                .ForMember(x => x.Birthdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<DriverForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 2));
            CreateMap<Account, DriverDto>();
            CreateMap<AccountForUpdateDto, Driver>();
        }
    }
}
