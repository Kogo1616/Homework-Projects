using Girteka.ElectricAggregate.Domain;
using Girteka.ElectricAggregate.Integrations;

namespace Girteka.ElectricAggregate.Job;

public class ElectryCityJob
{
    private readonly IContext<string, string, Stream> _context;
    private readonly IStorage<string, Stream> _storage;
    private readonly ILogger<ElectryCityJob> _logger;

    public ElectryCityJob(IContext<string, string, Stream> context, IStorage<string, Stream> storage,
        ILogger<ElectryCityJob> logger)
    {
        _context = context;
        _storage = storage;
        _logger = logger;
    }

    public void Do(string localPath, int? numberOfYears = null, int? numberOfMonths = null)
    {
        var uris = new HTTPLink().DownloadLastNPeriodFiles(numberOfYears, numberOfMonths);
        foreach (var uri in uris)
        {
            _logger.LogInformation("File start downloading");

            _context.Do(localPath, uri);

            _logger.LogInformation("File downloaded");
        }

        foreach (var uri in uris)
        {
            string fileName = uri.Split('/').Last();
            _storage.Do(localPath, fileName);

            _logger.LogInformation("Faile saved in DB");
        }
    }
}