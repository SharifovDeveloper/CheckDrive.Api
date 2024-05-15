using AutoMapper;
using CheckDrive.Domain.DTOs.Account;
using CheckDrive.Domain.Entities;

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
