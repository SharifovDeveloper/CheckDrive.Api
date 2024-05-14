using CheckDrive.Domain.Interfaces.Repositories;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Persistence.Repositories;
using CheckDrive.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CheckDrive.Api.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IDispatcherRepository, DispatcherRepository>();
            services.AddScoped<IDispatcherReviewRepository, DispatcherReviewRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorReviewRepository, DoctorReviewRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IMechanicRepository, MechanicRepository>();
            services.AddScoped<IMechanicAcceptenceRepository, MechanicAcceptenceRepository>();
            services.AddScoped<IMechanicHandoverRepository, MechanicHandoverRepository>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IOperatorReviewRepository, OperatorReviewRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IDispatcherService, DispatcherService>();
            services.AddScoped<IDispatcherReviewService, DispatcherReviewService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorReviewService, DoctorReviewService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IMechanicService, MechanicService>();
            services.AddScoped<IMechanicAcceptanceService, MechanicAcceptanceService>();
            services.AddScoped<IMechanicHandoverService, MechanicHandoverService>();
            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOperatorReviewService, OperatorReviewService>();

            services.AddControllers()
              .AddNewtonsoftJson(options =>
                  options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
               );

            return services;
        }
        public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services)
        {
            var builder = WebApplication.CreateBuilder();

            services.AddDbContext<CheckDriveDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CheckDriveConection")));

            return services;
        }
        public static IServiceCollection ConfigureLogger(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/error_.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return services;
        }
    }
}
