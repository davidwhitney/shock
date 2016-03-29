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
                env.Defibrillator.Shock(args);

                #if DEBUG
                Console.WriteLine("Press ANY key to exit.");
                Console.ReadKey();
                #endif
            }
        }
    }
}
