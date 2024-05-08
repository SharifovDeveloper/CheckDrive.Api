using CheckDriver.Domain.Entities;
using CheckDriver.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDricer.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : RepositoryBase<Doctor> ,IDoctorRepository
    {
        public DoctorRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
