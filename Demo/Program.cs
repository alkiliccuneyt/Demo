using System.Reflection;
using AutoMapper;
using Demo.Data.Configurations;
using Demo.Data.Configurations.Abstract;
using Demo.Data.Repositories.Abstract;
using Demo.Extensions;
using Demo.Repositories;
using Microsoft.Extensions.Options;
using Serilog;
#pragma warning disable CS8602

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
builder.Host.UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((config) =>
    {
        config
            .SetBasePath(Directory.GetParent(typeof(Program).Assembly.Location)?.FullName)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", true, true)
            .AddEnvironmentVariables();
    })
    .UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Add connection settings.
builder.Services.Configure<ConnectionConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<IConnectionConfiguration>(sp => sp.GetRequiredService<IOptions<ConnectionConfiguration>>().Value);
// Add database services.
builder.Services.AddTransient<IConnectionFactory, ConnectionFactory>();
builder.Services.AddTransient<IRepository, Repository>();
// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var config = new MapperConfiguration(cfg =>
{
    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
    cfg.AddProfile<MappingProfile>();
});
var mapper = config.CreateMapper();
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();