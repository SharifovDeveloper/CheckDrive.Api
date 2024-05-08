using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;

namespace CheckDricer.Infrastructure.Persistence.Repositories
{
    public class DispatcherRepository : RepositoryBase<Dispatcher>, IDispatcherRepository
    {
        public DispatcherRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
