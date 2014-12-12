namespace Likja.Tid.Logger
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] entities);
        void LogError(string message, params object[] entities);
        void LogWarning(string message, params object[] entities);
    }
}
