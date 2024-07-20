using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DriverRepository : RepositoryBase<Driver>, IDriverRepository
    {
        public DriverRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
