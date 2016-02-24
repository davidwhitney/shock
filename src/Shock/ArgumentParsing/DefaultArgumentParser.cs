namespace Shock.ArgumentParsing
{
    public class DefaultArgumentParser : IArgumentParser
    {
        public Arguments Parse(string[] raw)
        {
            if (raw == null) return new Arguments();

            var arguments = new Arguments(raw);

            foreach (var value in raw)
            {
                arguments.Add(value, new Arguments.Nothing());
            }

            return arguments;
        }
    }
}