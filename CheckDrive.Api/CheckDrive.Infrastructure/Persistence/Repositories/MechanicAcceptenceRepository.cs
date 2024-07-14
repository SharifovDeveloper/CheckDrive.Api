using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class MechanicAcceptenceRepository : RepositoryBase<MechanicAcceptance>, IMechanicAcceptenceRepository
    {
        public MechanicAcceptenceRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
