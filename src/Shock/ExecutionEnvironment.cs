using System;
using Shock.AppDomainShims;
using Shock.Conventions;
using Shock.EnvironmentDiscovery;
using Shock.Logging;

namespace Shock
{
    public class ExecutionEnvironment : IDisposable
    {
        public ActiveConventions Conventions { get; set; }
        public Defibrillator Defibrillator { get; set; }

        public ExecutionEnvironment(string[] cliArgs)
            : this(cliArgs, 
                  new DetectAndLoadRelevantAssemblies(new AppDomainWrapper(), new AssemblyWrapper(), new FileSystemWrapper(), new ConsoleOutput()),
                  new ConventionDiscoverer())
        {
        }

        public ExecutionEnvironment(string[] cliArgs, IDetectAndLoadRelevantAssemblies envLoader, IConventionDiscoverer conventionDiscoverer)
        {
            var loadedDomain = envLoader.LoadEnvironmentFrom(cliArgs);

            Conventions = ActiveConventions.Default();
            Conventions = conventionDiscoverer.AdjustConventions(Conventions, loadedDomain.GetAssemblies().AllTypes());

            Defibrillator = new Defibrillator(
                Conventions.TaskDiscoverer,
                Conventions.TaskSelector,
                Conventions.TaskRunner,
                Conventions.Output);

            // Detect execution environment and Conventions
            // Can we see log4net? nlog? If not, console + diagnostics writers
        }

        public void Dispose()
        {
        }
    }
}