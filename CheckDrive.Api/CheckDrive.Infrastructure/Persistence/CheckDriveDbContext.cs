using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CheckDrive.Infrastructure.Persistence
{
    public class CheckDriveDbContext : DbContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Mechanic> Mechanics { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<Dispatcher> Dispatchers { get; set; }
        public virtual DbSet<DoctorReview> DoctorReviews { get; set; }
        public virtual DbSet<OperatorReview> OperatorReviews { get; set; }
        public virtual DbSet<MechanicHandover> MechanicsHandovers { get; set; }
        public virtual DbSet<MechanicAcceptance> MechanicsAcceptances { get; set; }
        public virtual DbSet<DispatcherReview> DispatchersReviews { get; set; }

        public CheckDriveDbContext(DbContextOptions<CheckDriveDbContext> options)
            : base(options)
        {
            //Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
