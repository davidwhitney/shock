namespace Shock.Activation
{
    public interface IDependencyResolver
    {
        T Create<T>();
    }
}
