using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Entities;


namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<GetBaseResponse<AccountDto>> GetAccountsAsync(AccountResourceParameters resourceParameters);
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreate);
        Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdate);
        Task DeleteAccountAsync(int id);
        Task<Account> FindAccount(string login);
    }
}
