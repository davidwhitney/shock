using System;
using Shock.ArgumentParsing;

namespace Shock
{
    public class Program
    {
        public static void Main(string[] cliArgs)
        {
            var args = new Arguments(cliArgs);

            new DefibrillatorFactory()
                .Manufacture()
                .Shock(args);

            #if DEBUG
            Console.WriteLine("Press ANY key to exit.");
            Console.ReadKey();
            #endif
        }
    }
}
