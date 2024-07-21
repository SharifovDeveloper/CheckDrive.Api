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
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace CheckDrive.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration) 
        {
            AddProviders(services);
            AddDatabaseContext(services, configuration);
            AddRepositories(services);
            AddAutoMapper(services);
            AddServices(services);
            AddRealTimeHub(services);
            AddContentFormats(services);
            AddSwagger(services);
            AddAuthentication(services, configuration);
            AddAuthorization(services);

            return services;
        }

        private static void AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddSingleton<FileExtensionContentTypeProvider>();
        }

        private static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CheckDriveConnection");

            services.AddDbContext<CheckDriveDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static void AddRepositories(IServiceCollection services)
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
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddServices(IServiceCollection services)
        {
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
        }

        private static void AddRealTimeHub(IServiceCollection services)
        {
            services.AddScoped<IChatHub, ChatHub>();
            services.AddSignalR();
        }

        private static void AddContentFormats(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddXmlSerializerFormatters();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services
                .AddAuthentication(options =>
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
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
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
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("Admin", "true");
                })
                .AddPolicy("AdminOrDriver", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Driver" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrDoctor", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Doctor" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrOperator", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Operator" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrDispatcher", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Dispatcher" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrMechanic", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Mechanic" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });
        }

    }
}
