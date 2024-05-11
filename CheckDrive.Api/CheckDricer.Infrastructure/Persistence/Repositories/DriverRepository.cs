using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;
using CheckDriver.Infrastructure.Persistence;
using CheckDriver.Infrastructure.Persistence.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DriverRepository : RepositoryBase<Driver>, IDriverRepository
    {
        public DriverRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
