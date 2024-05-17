using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.ApiContracts.Account;



namespace CheckDrive.Domain.Mappings
{
    public class DispatcherMappings : Profile
    {
        public DispatcherMappings()
        {
            CreateMap<Dispatcher, DispatcherDto>()
                .ForMember(x => x.FirstName, e => e.MapFrom(f => f.Account.FirstName))
                .ForMember(x => x.LastName, e => e.MapFrom(f => f.Account.LastName))
                .ForMember(x => x.Login, e => e.MapFrom(f => f.Account.Login))
                .ForMember(x => x.Password, e => e.MapFrom(f => f.Account.Password))
                .ForMember(x => x.Birthdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<DispatcherForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 5));
            CreateMap<Account, DispatcherDto>();
            CreateMap<AccountForUpdateDto, Dispatcher>();
        }
    }
}
