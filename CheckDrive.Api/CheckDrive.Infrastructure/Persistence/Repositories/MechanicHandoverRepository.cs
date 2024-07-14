using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class MechanicHandoverRepository : RepositoryBase<MechanicHandover>, IMechanicHandoverRepository
    {
        public MechanicHandoverRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
