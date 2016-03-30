namespace Shock.Logging
{
    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string s)
        {
            System.Console.WriteLine(s);
        }
    }
}