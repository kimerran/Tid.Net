using System;
using Likja.Tid.Logger;

namespace Likja.Tid
{
    class Program
    {
        private static string command;
        private static ILogger logger;

        public static int Main(string[] args)
        {
            if (args.Length == 0) return TidRunner.ShowHelp();

            command = args[0];
            logger = new ConsoleLogger();

            if (args.Length < 2)
            {
                if (!IsShowOrHelp())
                {
                    logger.LogError(TidStrings.P1);
                    return ShowUsage();
                }
            }

            var parameter = (IsShowOrHelp()) ? "" : args[1];

            try
            {
                var runner = new TidRunner(logger);
                runner.Run(command, parameter);
            }
            catch (Exception ex)
            {
                logger.LogError("{0}\n{1}\n{2} {3}", TidStrings.P2, ex.Message, TidStrings.P3, string.Join(" ", args));

                return -2;
            }

            return 0;
        }

        private static int ShowUsage()
        {
            var usage = TidRunner.ShowUsage(command);
            if (usage != "")
                logger.LogWarning("{0} {1}\n", TidStrings.P4, usage);

            //TidRunner.ShowHelp();
            return -1;
        }

        internal static bool IsShowOrHelp()
        {
            return (command == TidStrings.P5 || command == TidStrings.P6);
        }

    }
}
