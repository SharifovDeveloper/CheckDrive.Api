using CheckDricer.Infrastructure.Persistence;
using CheckDricer.Infrastructure.Persistence.Repositories;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Services;
using CheckDriver.Domain.Interfaces.Repositories;
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

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICarService, CarService>();
            

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
