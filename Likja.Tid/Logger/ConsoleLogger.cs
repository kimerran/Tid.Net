using System;

namespace Likja.Tid.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message, params object[] entities)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message, entities);
            Console.ForegroundColor = oldColor;
        }

        public void LogError(string message, params object[] entities)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + message, entities);
            Console.ForegroundColor = oldColor;
        }

        public void LogWarning(string message, params object[] entities)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message, entities);
            Console.ForegroundColor = oldColor;
        }
    }
}
