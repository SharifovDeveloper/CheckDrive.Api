using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;
using CheckDriver.Infrastructure.Persistence;
using CheckDriver.Infrastructure.Persistence.Repositories;

namespace CheckDricer.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
