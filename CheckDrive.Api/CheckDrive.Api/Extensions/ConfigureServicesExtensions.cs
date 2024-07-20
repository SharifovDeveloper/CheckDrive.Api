using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Repositories;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Infrastructure.JwtToken;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Infrastructure.Persistence.Repositories;
using CheckDrive.Services;
using CheckDrive.Services.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
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
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IChatHub, ChatHub>();

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
                 .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 .WriteTo.Console(new RenderedCompactJsonFormatter())
                 .WriteTo.File(new RenderedCompactJsonFormatter(), "logs/logs.txt", rollingInterval: RollingInterval.Day)
                 .WriteTo.File(new RenderedCompactJsonFormatter(), "logs/error_.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
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
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Driver" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });

                options.AddPolicy("AdminOrDoctor", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Doctor" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });

                options.AddPolicy("AdminOrOperator", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Operator" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });

                options.AddPolicy("AdminOrDispatcher", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Dispatcher" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });

                options.AddPolicy("AdminOrMechanic", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Mechanic" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });
            });
        }
    }
}
