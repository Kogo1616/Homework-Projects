using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Girteka.ElectricAggregate.Persistence.Logger;

public class DatabaseLoggerProvider : ILoggerProvider
{
    private readonly Func<string, LogLevel, bool> _filter;
    private readonly string _connectionString;

    public DatabaseLoggerProvider(Func<string, LogLevel, bool> filter, string connectionString)
    {
        _filter = filter;
        _connectionString = connectionString;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(categoryName, _filter, _connectionString);
    }

    public void Dispose()
    {
    }
}