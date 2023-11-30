using System;

namespace EssayChecker.API.Brokers.Loggings;

public class LogBroker : ILoggingBroker
{
    public void LogInformation(string message)
    {
        throw new NotImplementedException();
    }

    public void LogTrace(string message)
    {
        throw new NotImplementedException();
    }

    public void LogDebug(string message)
    {
        throw new NotImplementedException();
    }

    public void LogWarning(string message)
    {
        throw new NotImplementedException();
    }

    public void LogError(Exception exception)
    {
        throw new NotImplementedException();
    }

    public void LogCritical(Exception exception)
    {
        throw new NotImplementedException();
    }
}