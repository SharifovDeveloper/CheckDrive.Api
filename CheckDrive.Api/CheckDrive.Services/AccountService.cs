using AutoMapper;
using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
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

        public async Task DeleteAccountAsync(int id)
        {
            var accountEntity = await _context.Accounts.FindAsync(id);

            if (accountEntity == null)
                throw new Exception("Account not found");

            await DelteAndCheckAccountRoles(id, accountEntity.RoleId);

            _context.Accounts.Remove(accountEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdate)
        {
            var accountEntity = _mapper.Map<Account>(accountForUpdate);

            _context.Accounts.Update(accountEntity);
            await _context.SaveChangesAsync();

            var accountDto = _mapper.Map<AccountDto>(accountEntity);

            return accountDto;
        }

        private async Task DelteAndCheckAccountRoles(int accountId, int role)
        {
            switch (role)
            {
                case 2:
                    var driver = await _context.Drivers
                               .Include(d => d.DoctorReviews)
                               .Include(ma => ma.MechanicAcceptance)
                               .Include(mh => mh.MechanicHandovers)
                               .Include(dr => dr.DispetcherReviews)
                               .Include(o => o.OperatorReviews)
                               .SingleOrDefaultAsync(d => d.AccountId == accountId);
                    if (driver != null)
                    {
                        _context.OperatorReviews.RemoveRange(driver.OperatorReviews);
                        _context.DoctorReviews.RemoveRange(driver.DoctorReviews);
                        _context.MechanicsAcceptances.RemoveRange(driver.MechanicAcceptance);
                        _context.MechanicsHandovers.RemoveRange(driver.MechanicHandovers);
                        _context.DispatchersReviews.RemoveRange(driver.DispetcherReviews);
                        _context.Drivers.Remove(driver);
                        await _context.SaveChangesAsync();
                    }
                    break;
                case 3:
                    var doctor = await _context.Doctors
                               .Include(d => d.DoctorReviews)
                               .SingleOrDefaultAsync(d => d.AccountId == accountId);
                    if (doctor != null)
                    {
                        _context.DoctorReviews.RemoveRange(doctor.DoctorReviews);
                        _context.Doctors.Remove(doctor);
                        await _context.SaveChangesAsync();
                    }
                    break;
                case 4:
                    var _operator = await _context.Operators
                               .Include(o => o.OperatorReviews)
                               .Include(d => d.DispetcherReviews)
                               .SingleOrDefaultAsync(d => d.AccountId == accountId);
                    if (_operator != null)
                    {
                        _context.DispatchersReviews.RemoveRange(_operator.DispetcherReviews);
                        _context.OperatorReviews.RemoveRange(_operator.OperatorReviews);
                        _context.Operators.Remove(_operator);
                        await _context.SaveChangesAsync();
                    }
                    break;
                case 5:
                    var dispatcher = await _context.Dispatchers
                               .Include(o => o.DispetcherReviews)
                               .SingleOrDefaultAsync(d => d.AccountId == accountId);
                    if (dispatcher != null)
                    {
                        _context.DispatchersReviews.RemoveRange(dispatcher.DispetcherReviews);
                        _context.Dispatchers.Remove(dispatcher);
                        await _context.SaveChangesAsync();
                    }
                    break;
                case 6:
                    var mechanic = await _context.Mechanics
                               .Include(o => o.MechanicAcceptance)
                               .Include(o => o.MechanicHandovers)
                               .Include(d => d.DispetcherReviews)
                               .SingleOrDefaultAsync(d => d.AccountId == accountId);
                    if (mechanic != null)
                    {
                        _context.MechanicsAcceptances.RemoveRange(mechanic.MechanicAcceptance);
                        _context.MechanicsHandovers.RemoveRange(mechanic.MechanicHandovers);
                        _context.DispatchersReviews.RemoveRange(mechanic.DispetcherReviews);
                        _context.Mechanics.Remove(mechanic);
                        await _context.SaveChangesAsync();
                    }
                    break;
            }
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

            if (resourceParameters.BirthDate is not null)
            {
                query = query.Where(x => x.Bithdate.Date == resourceParameters.BirthDate.Value.Date);
            }
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
