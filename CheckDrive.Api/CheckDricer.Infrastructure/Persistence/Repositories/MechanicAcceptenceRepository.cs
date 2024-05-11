using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;
using CheckDriver.Infrastructure.Persistence;
using CheckDriver.Infrastructure.Persistence.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class MechanicAcceptenceRepository : RepositoryBase<MechanicAcceptance>, IMechanicAcceptenceRepository
    {
        public MechanicAcceptenceRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
