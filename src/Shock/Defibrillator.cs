using System.Collections.Generic;
using System.Linq;
using Shock.ArgumentParsing;
using Shock.Discovery;
using Shock.Execution;

namespace Shock
{
    public class Defibrillator
    {
        private readonly IDiscoverTasks _discoverer;
        private readonly ISelectTasksToRun _taskSelector;
        private readonly IExecuteATask _executor;

        public Defibrillator(
            IDiscoverTasks discoverer,
            ISelectTasksToRun taskSelector,
            IExecuteATask executor)
        {
            _discoverer = discoverer;
            _taskSelector = taskSelector;
            _executor = executor;
        }
        
        public void Shock(Arguments args)
        {

            var allTasks = _discoverer.FindTasks(args);
            var tasksToExecute = _taskSelector.SelectTasksFrom(allTasks, args);
            var results = tasksToExecute.Select(t => _executor.TryExecuteTask(t, args)).ToList();
        }
    }
}