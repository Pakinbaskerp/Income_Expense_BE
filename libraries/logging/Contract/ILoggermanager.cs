namespace libraries.logging.Contract;

public interface ILoggerManager<T>
{
    void LogInfo(string logMessage, params object[] args);
    void LogError(Exception exception,string logError, params object[] args);

}