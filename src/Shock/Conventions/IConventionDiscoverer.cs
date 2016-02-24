namespace Shock.Conventions
{
    public interface IConventionDiscoverer
    {
        ActiveConventions AdjustConventions(ActiveConventions conventions);
    }
}