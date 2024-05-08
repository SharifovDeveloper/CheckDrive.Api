using CheckDricer.Infrastructure.Persistence;
using CheckDricer.Infrastructure.Persistence.Repositories;
using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DoctorReviewRepository : RepositoryBase<DoctorReview>, IDoctorReviewRepository
    {
        public DoctorReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
