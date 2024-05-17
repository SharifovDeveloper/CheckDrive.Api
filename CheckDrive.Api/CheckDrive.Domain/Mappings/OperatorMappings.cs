using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.Operator;
using CheckDrive.ApiContracts.Account;



namespace CheckDrive.Domain.Mappings
{
    public class OperatorMappings : Profile
    {
        public OperatorMappings()
        {
            CreateMap<Operator, OperatorDto>()
                .ForMember(x => x.FirstName, e => e.MapFrom(f => f.Account.FirstName))
                .ForMember(x => x.LastName, e => e.MapFrom(f => f.Account.LastName))
                .ForMember(x => x.Login, e => e.MapFrom(f => f.Account.Login))
                .ForMember(x => x.Password, e => e.MapFrom(f => f.Account.Password))
                .ForMember(x => x.Birthdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<OperatorForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 4));
            CreateMap<Account, OperatorDto>();
            CreateMap<AccountForUpdateDto, Operator>();
        }
    }
}
