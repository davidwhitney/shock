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
            Parse(raw);
        }

        private void Parse(string[] raw)
        {
            if (raw == null) return;

            foreach (var value in raw)
            {
                Add(value, new Nothing());
            }
        }

        public static Arguments With(string argument)
        {
            return new Arguments {{argument, new Nothing()}};
        }

        public class Nothing { }
    }
}
