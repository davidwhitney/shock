using System;

namespace Shock
{
    public class Program
    {
        public static void Main(string[] cliArgs)
        {
            using (var env = new ExecutionEnvironment(cliArgs))
            {
                var args = env.Conventions.ArgumentParser.Parse(cliArgs);
                Environment.ExitCode = (int)env.Defibrillator.Shock(args);

                if (args.Interactive)
                {
                    Console.WriteLine("Press ANY key to exit.");
                    Console.ReadKey();
                }
            }
        }
    }
}
