using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Persistence.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DispatcherRepository : RepositoryBase<Dispatcher>, IDispatcherRepository
    {
        public DispatcherRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
