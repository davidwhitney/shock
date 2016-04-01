using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.AppDomainShims;
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
        private readonly IOutput _output;
        private readonly UsageExamples _usage;

        public IExecuteATask Executor { get; set; }
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
            Executor = executor;
            _output = output;
            _usage = new UsageExamples(_output);
        }
        
        public ExitCodes Shock(Arguments args)
        {
            DiscoveredTasks = _discoverer.FindTasks(args);
            if (!DiscoveredTasks.Any())
            {
                _usage.Basic();
                return ExitCodes.NoTasksRun;
            }

            SelectedTasks = _taskSelector.SelectTasksFrom(DiscoveredTasks, args);

            if (args.Help || !SelectedTasks.Any())
            {
                _usage.Tasks(DiscoveredTasks);
                return ExitCodes.NoTasksRun;
            }

            foreach (var task in SelectedTasks)
            {
                var result = Executor.TryExecuteTask(task, args);
                Results.Add(result);
                _output.WriteLine($"{(result.ExecutedSuccessfully ? "Executed" : "Failed")}: {result.Method.DeclaringType.FullName}.{result.Method.Name}");

                if (!result.ExecutedSuccessfully)
                {
                    _output.WriteLine(result.Exception.ToString());
                }

                if (!result.ExecutedSuccessfully && !args.Continue)
                {
                    return ExitCodes.Failed;
                }
            }

            return ExitCodes.Success;
        }
    }

    public enum ExitCodes
    {
        Failed = -1,
        Success = 0,
        NoTasksRun = 1,
    }
}