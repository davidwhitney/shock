using System.Collections.Generic;

namespace Shock.Logging
{
    public class ListOutput : IOutput
    {
        public List<string> Buffer { get; set; } = new List<string>();

        public void WriteLine(string s)
        {
            Buffer.Add(s);
        }
    }
}