using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Repositories;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.JwtToken;
using CheckDrive.Infrastructure.PasswordHash;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Persistence.Repositories;
using CheckDrive.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
            services.AddScoped<IAuthorizationService, AuthorizationService>();
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

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

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

        public static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var JwtOption = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(JwtOption!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty-cookies"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("Admin", "true");
                });

                options.AddPolicy("AdminOrDriver", policy =>
                {
                    policy.RequireClaim("Driver", "true");
                    policy.RequireClaim("Admin", "true");
                });

                options.AddPolicy("AdminOrDoctor", policy =>
                {
                    policy.RequireClaim("Doctor", "true");
                    policy.RequireClaim("Admin", "true");
                });

                options.AddPolicy("AdminOrOperator", policy =>
                {
                    policy.RequireClaim("Operator", "true");
                    policy.RequireClaim("Admin", "true");
                });

                options.AddPolicy("AdminOrDispatcher", policy =>
                {
                    policy.RequireClaim("Admin", "true");
                    policy.RequireClaim("Dispatcher", "true");
                });

                options.AddPolicy("AdminOrMechanic", policy =>
                {
                    policy.RequireClaim("Mechanic", "true");
                    policy.RequireClaim("Admin", "true");
                });
            });
        }
    }
}
