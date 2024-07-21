using CheckDrive.Infrastructure.JwtToken;

namespace CheckDrive.Api.Extensions;

public static class Configurations
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        AddJwtOptions(services, configuration);

        return services;
    }

    private static void AddJwtOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(nameof(JwtOptions)));
    }
}
