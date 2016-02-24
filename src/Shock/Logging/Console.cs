namespace Shock.Logging
{
    public class Console : IOutput
    {
        public void WriteLine(string s)
        {
            System.Console.WriteLine(s);
        }
    }
}