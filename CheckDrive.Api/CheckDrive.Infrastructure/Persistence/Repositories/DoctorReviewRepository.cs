using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DoctorReviewRepository : RepositoryBase<DoctorReview>, IDoctorReviewRepository
    {
        public DoctorReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
