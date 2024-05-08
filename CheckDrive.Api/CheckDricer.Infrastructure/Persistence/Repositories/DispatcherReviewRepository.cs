using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;

namespace CheckDricer.Infrastructure.Persistence.Repositories
{
    public class DispatcherReviewRepository : RepositoryBase<DispatcherReview>, IDispatcherReviewRepository
    {
        public DispatcherReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
