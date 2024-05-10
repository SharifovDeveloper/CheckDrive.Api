using CheckDrive.Api.Extensions;
using CheckDrive.Api.Middlewares;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
