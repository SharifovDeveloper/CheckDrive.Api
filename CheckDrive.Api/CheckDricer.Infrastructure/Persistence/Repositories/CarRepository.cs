using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;

namespace CheckDricer.Infrastructure.Persistence.Repositories
{
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
