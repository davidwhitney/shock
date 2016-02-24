using System.Collections.Generic;

namespace Shock.ArgumentParsing
{
    public class Arguments : Dictionary<string, object>
    {
        public string[] Raw { get; private set; }

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
    }
}
