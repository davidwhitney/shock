using System.Linq;

namespace Shock.ArgumentParsing
{
    public class DefaultArgumentParser : IArgumentParser
    {
        public Arguments Parse(string[] raw)
        {
            if (raw == null) return new Arguments();

            var arguments = new Arguments(raw);

            for (int index = 0; index < raw.Length; index++)
            {
                var value = raw[index];

                new [] 
                {
                    "--",
                    "-",
                    "/"
                }.ToList().ForEach(prefix => value = value.StartsWith(prefix) ? value.Substring(prefix.Length) : value);

                if (value.Contains("="))
                {
                    var parts = value.Split('=');
                    arguments.Add(parts[0], parts[1]);
                }
                else
                {
                    arguments.Add(value, new Arguments.Nothing());
                }
            }

            return arguments;
        }
    }
}