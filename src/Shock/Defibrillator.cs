using System;
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
        }
        
        public void Shock(Arguments args)
        {
            var allTasks = _discoverer.FindTasks(args);
            var tasksToExecute = _taskSelector.SelectTasksFrom(allTasks, args);
            NotifyIfAllTasksFiltered(allTasks, tasksToExecute);

            var results = tasksToExecute.Select(t => _executor.TryExecuteTask(t, args)).ToList();
            ReportResults(results);
        }

        private void NotifyIfAllTasksFiltered(List<MethodInfo> allTasks, List<MethodInfo> tasksToExecute)
        {
            if (allTasks.Count <= 0 || tasksToExecute.Count != 0) return;

            _output.WriteLine("tasks:" + Environment.NewLine);
            foreach (var task in allTasks)
            {
                _output.WriteLine(
                    $"\t{task.DeclaringType.Namespace}.{task.DeclaringType.Name}.{task.Name}{Environment.NewLine}");
            }
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