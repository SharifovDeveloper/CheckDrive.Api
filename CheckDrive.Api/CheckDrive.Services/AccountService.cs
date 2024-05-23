using AutoMapper;
using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;

        public AccountService(IMapper mapper, CheckDriveDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetBaseResponse<AccountDto>> GetAccountsAsync(AccountResourceParameters resourceParameters)
        {
            var query = GetQueryAccountResParameters(resourceParameters);

            var accounts = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var accountDtos = _mapper.Map<List<AccountDto>>(accounts);

            var paginatedResult = new PaginatedList<AccountDto>(accountDtos, accounts.TotalCount, accounts.CurrentPage, accounts.PageSize);

            return paginatedResult.ToResponse();
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);

            var accountDto = _mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreate)
        {
            var createdAccount = await CreateAndCheckAccountRoles(accountForCreate);

            var accountDto = _mapper.Map<AccountDto>(createdAccount);

            return accountDto;
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdate)
        {
            var accountEntity = _mapper.Map<Account>(accountForUpdate);

            _context.Accounts.Update(accountEntity);
            await _context.SaveChangesAsync();

            var accountDto = _mapper.Map<AccountDto>(accountEntity);

            return accountDto;
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (account is not null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Account> CreateAndCheckAccountRoles(AccountForCreateDto accountForCreate)
        {
            var accountEntity = _mapper.Map<Account>(accountForCreate);
            await _context.Accounts.AddAsync(accountEntity);
            await _context.SaveChangesAsync();

            switch (accountForCreate.RoleId)
            {
                case 2:
                    var driver = new Driver { AccountId = accountEntity.Id };
                    await _context.Drivers.AddAsync(driver);
                    break;
                case 3:
                    var doctor = new Doctor { AccountId = accountEntity.Id };
                    await _context.Doctors.AddAsync(doctor);
                    break;
                case 4:
                    var _operator = new Operator { AccountId = accountEntity.Id };
                    await _context.Operators.AddAsync(_operator);
                    break;
                case 5:
                    var dispatcher = new Dispatcher { AccountId = accountEntity.Id };
                    await _context.Dispatchers.AddAsync(dispatcher);
                    break;
                case 6:
                    var mechanic = new Mechanic { AccountId = accountEntity.Id };
                    await _context.Mechanics.AddAsync(mechanic);
                    break;
            }
            await _context.SaveChangesAsync();

            return accountEntity;
        }
        private IQueryable<Account> GetQueryAccountResParameters(
          AccountResourceParameters resourceParameters)
        {
            var query = _context.Accounts.Include(x => x.Role).AsQueryable();

            if (resourceParameters.Login is not null)
            {
                query = query.Where(x => x.Login == resourceParameters.Login);
            }
            if (resourceParameters.Pasword is not null)
            {
                query = query.Where(x => x.Password == resourceParameters.Pasword);
            }
            if (resourceParameters.RoleId != 0 && resourceParameters.RoleId is not null)
            {
                query = query.Where(x => x.RoleId == resourceParameters.RoleId);
            }
            if (!string.IsNullOrWhiteSpace(resourceParameters.SearchString))
            {
                query = query.Where(x => x.FirstName.Contains(resourceParameters.SearchString)
                || x.LastName.Contains(resourceParameters.SearchString));
            }
            if (resourceParameters.RoleId != 0 && resourceParameters.RoleId is not null)
            {
                query = query.Where(x => x.RoleId == resourceParameters.RoleId);
            }
            if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            {
                query = resourceParameters.OrderBy.ToLowerInvariant() switch
                {
                    "firstname" => query.OrderBy(x => x.FirstName),
                    "firstnamedesc" => query.OrderByDescending(x => x.FirstName),
                    "lastname" => query.OrderBy(x => x.LastName),
                    "lastnamedesc" => query.OrderByDescending(x => x.LastName),
                    "login" => query.OrderBy(x => x.Login),
                    "logindesc" => query.OrderByDescending(x => x.Login),
                    "phonenumber" => query.OrderBy(x => x.PhoneNumber),
                    "phonenumberdesc" => query.OrderByDescending(x => x.PhoneNumber),
                    _ => query.OrderBy(x => x.Id),
                };
            }
            return query;
        }
    }
}
