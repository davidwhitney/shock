namespace Shock.ArgumentParsing
{
    public interface IArgumentParser
    {
        Arguments Parse(string[] raw);
    }
}