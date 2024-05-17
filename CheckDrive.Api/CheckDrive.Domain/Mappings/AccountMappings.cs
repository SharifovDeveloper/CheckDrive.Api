using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.Account;

namespace CheckDrive.Domain.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>();
            CreateMap<AccountForCreateDto, Account>();
            CreateMap<AccountForCreateDto, Driver>();
            CreateMap<AccountForUpdateDto, Account>();
        }
    }
}
