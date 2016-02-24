using System.Collections.Generic;
using System.Linq;
using Shock.ArgumentParsing;
using Shock.Execution;
using Shock.Logging;
using Shock.TaskDiscovery;

namespace Shock
{
    public class Defibrillator
    {
        private readonly IDiscoverTasks _discoverer;
        private readonly ISelectTasksToRun _taskSelector;
        private readonly IExecuteATask _executor;
        private readonly IOutput _output;
        private readonly UsageExamples _usage;

        public Defibrillator(
            IDiscoverTasks discoverer,
            ISelectTasksToRun taskSelector,
            IExecuteATask executor,
            IOutput output)
        {
            _discoverer = discoverer;
            _taskSelector = taskSelector;
            _executor = executor;
            _output = output;
            _usage = new UsageExamples(_output);
        }
        
        public void Shock(Arguments args)
        {
            var allTasks = _discoverer.FindTasks(args);
            if (!allTasks.Any())
            {
                _usage.Basic();
                return;
            }

            var tasksToExecute = _taskSelector.SelectTasksFrom(allTasks, args);
            if (tasksToExecute.Count == 0)
            {
                _usage.Tasks(allTasks);
                return;
            }

            var results = tasksToExecute.Select(t => _executor.TryExecuteTask(t, args)).ToList();
            ReportResults(results);
        }

        private void ReportResults(List<TaskStatus> results)
        {
            foreach (var result in results)
            {
                var msg = result.ExecutedSuccessfully ? "Executed" : "Failed";
                _output.WriteLine($"{msg}: {result.Method.DeclaringType.FullName}.{result.Method.Name}");
            }
        }
    }
}