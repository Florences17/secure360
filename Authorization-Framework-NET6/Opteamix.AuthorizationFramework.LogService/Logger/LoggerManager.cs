using NLog;
using Opteamix.AuthorizationFramework.LogService.Interface;
using System;

namespace Opteamix.AuthorizationFramework.LogService.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        public void LogError(string message,Exception ex)
        {
            logger.Error(ex,message);
        }
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
