using Girteka.ElectricAggregate.Domain.Models;

namespace Girteka.ElectricAggregate.Domain;

public class FileArchive : IFileArchive
{
    private readonly IDbContext _dbContext;


    public FileArchive(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveFileInArchive(string fileName)
    {
        _dbContext.FileLogs.Add(new FileLog()
        {
            FileName = fileName,
            CreateDateTime = DateTime.Now,
            Status = "Donwloaded"
        });

        await _dbContext.SaveChangesAsync();
    }
}