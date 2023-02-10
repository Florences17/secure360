using System;

namespace Opteamix.AuthorizationFramework.LogService.Interface
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message, Exception ex);
    }
}
