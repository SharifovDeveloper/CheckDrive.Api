using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class OperatorRepository : RepositoryBase<Operator>, IOperatorRepository
    {
        public OperatorRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
