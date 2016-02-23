using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Execution
{
    public class SelectTasksToRun : ISelectTasksToRun
    {
        public List<MethodInfo> SelectTasksFrom(List<MethodInfo> tasksFromDomain, Arguments args)
        {
            var matchingTasks = new List<MethodInfo>();
            matchingTasks.AddRange(tasksFromDomain.Where(x => args.Keys.Contains(x.Name)));

            return matchingTasks;
        }
    }
}