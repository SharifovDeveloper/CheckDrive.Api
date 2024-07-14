using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DispatcherRepository : RepositoryBase<Dispatcher>, IDispatcherRepository
    {
        public DispatcherRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
