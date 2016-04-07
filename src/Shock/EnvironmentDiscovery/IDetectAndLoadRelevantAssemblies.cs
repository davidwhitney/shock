using System;

namespace Shock.EnvironmentDiscovery
{
    public interface IDetectAndLoadRelevantAssemblies
    {
        AppDomain LoadEnvironmentFrom(string[] args);
        string ActiveAppConfiguration { get; }
    }
}
