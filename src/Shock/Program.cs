using System;

namespace Shock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Defibrillator(args).Shock();

            #if DEBUG
            Console.WriteLine("Press ANY key to exit.");
            Console.ReadKey();
            #endif
        }
    }
}
