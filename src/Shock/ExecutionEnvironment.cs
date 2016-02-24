using System;
using Shock.Conventions;
using Shock.EnvironmentDiscovery;

namespace Shock
{
    public class ExecutionEnvironment : IDisposable
    {
        public ActiveConventions Conventions { get; set; }
        public DefibrillatorFactory DefibrillatorFactory { get; set; }

        public ExecutionEnvironment(string[] cliArgs)
        {
            var environmentalLoader = new DetectAndLoadRelevantAssemblies();
            environmentalLoader.LoadEnvironmentFrom(cliArgs);

            Conventions = ActiveConventions.Default();
            Conventions = new ConventionDiscoverer().AdjustConventions(Conventions);
            DefibrillatorFactory = new DefibrillatorFactory(Conventions);
        }

        public void Dispose()
        {
        }
    }
}