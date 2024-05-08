using CheckDricer.Infrastructure.Persistence;
using CheckDricer.Infrastructure.Persistence.Repositories;
using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class OperatorReviewRepository : RepositoryBase<OperatorReview>, IOperatorReviewRepository
    {
        public OperatorReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
