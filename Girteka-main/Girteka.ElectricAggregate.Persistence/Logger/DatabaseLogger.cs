using Girteka.ElectricAggregate.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Girteka.ElectricAggregate.Persistence.Logger;

public class DatabaseLogger : ILogger
{
    private readonly string _categoryName;
    private readonly Func<string, LogLevel, bool> _filter;
    private readonly string _connectionString;

    public DatabaseLogger(string categoryName, Func<string, LogLevel, bool> filter, string connectionString)
    {
        _categoryName = categoryName;
        _filter = filter;
        _connectionString = connectionString;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _filter == null || _filter(_categoryName, logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var logMessage = formatter(state, exception);

        using (var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
        {
            context.Logs.Add(new Log
            {
                Timestamp = DateTime.UtcNow,
                Message = logMessage,
            });

            context.SaveChanges();
        }
    }
}