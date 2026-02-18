using libraries.logging.Contract;
using Microsoft.Extensions.Logging;


namespace libraries.logging.Infrastructure;

public class LoggerManager<T> : ILoggerManager<T>
{
    private readonly ILogger<T> _logger;

    public LoggerManager(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogError(Exception exception, string logError, params object[] args)
    {
       _logger.LogError(exception, logError, args);
    }

    public void LogInfo(string logMessage, params object[] args)
    {
       _logger.LogInformation(logMessage, args);
    }
}