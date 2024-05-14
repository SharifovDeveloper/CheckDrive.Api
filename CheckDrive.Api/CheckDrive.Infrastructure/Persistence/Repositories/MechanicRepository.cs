using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Persistence.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class MechanicRepository : RepositoryBase<Mechanic>, IMechanicRepository
    {
        public MechanicRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
