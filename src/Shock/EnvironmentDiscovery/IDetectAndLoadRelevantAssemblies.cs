using System;

namespace Shock.EnvironmentDiscovery
{
    public interface IDetectAndLoadRelevantAssemblies
    {
        void LoadEnvironmentFrom(string[] args);
    }
}
