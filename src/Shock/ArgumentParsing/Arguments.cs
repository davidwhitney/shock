using System.Collections.Generic;
using System.Linq;

namespace Shock.ArgumentParsing
{
    public class Arguments : Dictionary<string, object>
    {
        public string[] Raw { get; set; }

        public Arguments()
        {
            Raw = new string[0];
        }

        public Arguments(string[] raw)
        {
            Raw = raw ?? new string[0];
        }

        public static Arguments With(string argument, object value = null)
        {
            value = value ?? new Nothing();

            return new Arguments {{argument, value}};
        }

        public class Nothing { }

        public bool Verbose => Keys.Any(k => k.ToLower() == "verbose" || k.ToLower() == "v");
        public bool Continue => Keys.Any(k => k.ToLower() == "continue");
        public bool Help => Keys.Any(k => k.ToLower() == "help" || k == "?");
    }
}
