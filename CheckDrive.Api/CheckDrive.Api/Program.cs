using CheckDrive.Api.Extensions;
using CheckDrive.Api.Middlewares;
using CheckDrive.Infrastructure.JwtToken;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Configuration;
using CheckDrive.Services.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseSerilog();
// Add services to the container.

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer()
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddSingleton<FileExtensionContentTypeProvider>()
        .ConfigureLogger()
        .ConfigureRepositories()
        .ConfigureDatabaseContext()
        .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiAuthentication(configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    builder.Services.SeedDatabase(services);
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");
app.MapControllers();


app.Run();
