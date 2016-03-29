using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public List<MethodInfo> DiscoveredTasks { get; private set; } = new List<MethodInfo>(); 
        public List<MethodInfo> SelectedTasks { get; private set; } = new List<MethodInfo>();
        public List<TaskStatus> Results { get; private set; } = new List<TaskStatus>();

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
            DiscoveredTasks = _discoverer.FindTasks(args);
            if (!DiscoveredTasks.Any())
            {
                _usage.Basic();
                return;
            }

            SelectedTasks = _taskSelector.SelectTasksFrom(DiscoveredTasks, args);
            if (SelectedTasks.Count != 1)
            {
                _usage.Tasks(DiscoveredTasks);
                return;
            }

            Results = SelectedTasks.Select(t => _executor.TryExecuteTask(t, args)).ToList();
            ReportResults();
        }

        private void ReportResults()
        {
            foreach (var result in Results)
            {
                var msg = result.ExecutedSuccessfully ? "Executed" : "Failed";
                _output.WriteLine($"{msg}: {result.Method.DeclaringType.FullName}.{result.Method.Name}");
            }
        }
    }
}