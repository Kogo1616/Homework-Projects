using Girteka.ElectricAggregate.Domain.Services;
using Girteka.ElectricAggregate.Integrations;
using Girteka.ElectricAggregate.Persistence;
using Quartz;

namespace Girteka.ElectricAggregate.Job;

public class ElectricityDataDownloaderJob : IJob
{
    private readonly string _csvLocalpPath;
    private readonly ILogger<ElectricityDataDownloaderJob> _logger;

    public ElectricityDataDownloaderJob(IConfiguration configuration, ILogger<ElectricityDataDownloaderJob> logger)
    {
        _logger = logger;
        _csvLocalpPath = configuration.GetValue<string>("CsvLocalpPath");
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Job start work");

            new ElectryCityJob(
                new DownloadFiles(
                    new CSVFileFromHTTTP(new HttpClient())
                ),
                new StoreFiles(
                    new CSVFileFromLocalDisk(),
                    new ApplicationDbContext()
                )
                , new Logger<ElectryCityJob>(new LoggerFactory())
            ).Do(_csvLocalpPath,
                2); //the second parameter is the data we want to get, for example if we set it to 2 we get 2 years old data

            _logger.LogInformation("Job completed successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}