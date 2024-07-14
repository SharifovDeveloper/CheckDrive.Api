using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DispatcherReviewRepository : RepositoryBase<DispatcherReview>, IDispatcherReviewRepository
    {
        public DispatcherReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
