using Girteka.ElectricAggregate.Domain;
using Girteka.ElectricAggregate.Persistence;
using Girteka.ElectricAggregate.Persistence.Logger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Services.AddScoped<IDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IElectricity, ElectricityRepository>();
builder.Services.AddScoped<IFileArchive, FileArchive>();
builder.Services.AddHttpClient(); // Register HttpClient

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            configuration.GetConnectionString("ConnectionString"),
            x => x.MigrationsAssembly("Girteka.ElectricAggregate.Persistence")));

builder.Services.AddLogging(builder =>
{
    builder.AddProvider(new DatabaseLoggerProvider(
        (category, level) => level >= LogLevel.Information,
        configuration.GetConnectionString("ConnectionString")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();