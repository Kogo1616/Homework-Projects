using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Girteka.ElectricAggregate.Domain.Mappers;
using Girteka.ElectricAggregate.Domain.Models;

namespace Girteka.ElectricAggregate.Domain.Services;

public class StoreFiles : IStorage<string, Stream>
{
    private readonly IContext<string, string, Stream> _context;
    private readonly IDbContext _dbContext;

    public StoreFiles(IContext<string, string, Stream> context, IDbContext dbContext)
    {
        _context = context;
        _dbContext = dbContext;
    }

    public void Do(string param, string fileName)
    {
        var convertedFile = ConvertStreamToModel(_context.Do(fileName, param));

        DateTime result = DateTime.ParseExact(fileName.Substring(0, fileName.LastIndexOf(".")), "yyyy-MM",
            CultureInfo.InvariantCulture);

        var filteredData = convertedFile
            .Where(x => x.Pavadinimas == "Butas")
            .GroupBy(x => x.Tinklas)
            .Select(s => new Electricity()
            {
                Tinklas = s.Key,
                Pavadinimas = "Butas",
                PPlus = s.Sum(x => x.PPlus),
                PMinus = s.Sum(x => x.PMinus),
                PlT = result
            });

        _dbContext.Electricities.AddRangeAsync(filteredData);

        _dbContext.SaveChangesAsync();
    }

    private List<Electricity> ConvertStreamToModel(Stream stream)
    {
        var response = new List<Electricity>();

        using (var reader = new StreamReader(stream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };
            csv.Context.RegisterClassMap<ElectricityMapper>();
            response.AddRange(csv.GetRecords<Electricity>());
        }

        return response;
    }
}