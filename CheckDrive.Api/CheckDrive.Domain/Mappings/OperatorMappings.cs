using AutoMapper;
using CheckDrive.Domain.DTOs.Account;
using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.Entities;

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
                .ForMember(x => x.Bithdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<OperatorForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 4));
            CreateMap<Account, OperatorDto>();
            CreateMap<AccountForUpdateDto, Operator>();
        }
    }
}
