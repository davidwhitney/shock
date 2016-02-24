using System;

namespace Shock.Activation
{
    public class ActivatorDependencyResolver : IDependencyResolver
    {
        public T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}