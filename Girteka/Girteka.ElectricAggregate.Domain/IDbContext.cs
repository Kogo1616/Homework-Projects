using Girteka.ElectricAggregate.Domain.Models;
using Girteka.ElectricAggregate.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Girteka.ElectricAggregate.Domain;

public interface IDbContext
{
    public DbSet<Electricity> Electricities { get; set; }

    public DbSet<Log> Logs { get; set; }
    
    public DbSet<FileLog> FileLogs { get; set; }

    public Task SaveChangesAsync();
}