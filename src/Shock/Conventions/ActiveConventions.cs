using Shock.Activation;
using Shock.ArgumentParsing;
using Shock.Execution;
using Shock.Logging;
using Shock.TaskDiscovery;

namespace Shock.Conventions
{
    public class ActiveConventions
    {
        public IDependencyResolver DependencyResolver { get; set; }
        public IArgumentParser ArgumentParser { get; set; }
        public IExecuteATask TaskRunner { get; set; }
        public IDiscoverTasks TaskDiscoverer { get; set; }
        public ISelectTasksToRun TaskSelector { get; set; }
        public IOutput Output { get; set; }

        public static ActiveConventions Default()
        {
            return new ActiveConventions
            {
                DependencyResolver = new ActivatorDependencyResolver(),
                ArgumentParser = new DefaultArgumentParser(),
                TaskRunner = new DefaultTaskExecutor(),
                TaskDiscoverer = new DefaultTaskDiscoverer(),
                TaskSelector = new SelectTasksToRun(),
                Output = new Console()
            };
        }
    }
}
