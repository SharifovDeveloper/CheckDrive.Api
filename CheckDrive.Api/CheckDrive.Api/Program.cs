using CheckDrive.Api.Extensions;
using CheckDrive.Services.Hubs;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.Console(new CustomJsonFormatter())
    .WriteTo.File(new CustomJsonFormatter(), "logs/logs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.File(new CustomJsonFormatter(), "logs/error_.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddConfigurationOptions(configuration)
    .ConfigureServices(configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    builder.Services.SeedDatabase(services);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<ChatHub>("api/chat");

app.MapControllers();

app.Run();
