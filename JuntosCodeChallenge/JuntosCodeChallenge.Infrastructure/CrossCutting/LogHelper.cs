using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace JuntosCodeChallenge.Infrastructure.CrossCutting
{
    public class LogHelper
    {
        public LogHelper()
        {}

        public void InitializeFile(ILoggerFactory loggerFactory, string filePath, string fileExtension)
        {
            Log.Logger = new LoggerConfiguration()
           .WriteTo.Console()
           .WriteTo.File($"{filePath}JuntosCodeChallenge-{fileExtension}", rollingInterval: RollingInterval.Day)
           .CreateLogger();
        }

        public void Information(string logMessage)
        {
            Log.Information(logMessage);
        }

        public void Warning(string logMessage)
        {
            Log.Warning(logMessage);
        }

        public void Error(Exception e, string logMessage)
        {
            Log.Error(e, logMessage);
        }

        public void Error(Exception e, string logMessage, object o)
        {
            Log.Error(e, logMessage, o);
        }

        public void Debug(string logMessage)
        {
            Log.Debug(logMessage);
        }

        public void Fatal(string logMessage)
        {
            Log.Fatal(logMessage);
        }
    }
}