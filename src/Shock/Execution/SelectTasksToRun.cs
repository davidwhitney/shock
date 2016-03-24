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
            matchingTasks.AddRange(tasksFromDomain.Where(x => MatchesName(args, x)));

            return matchingTasks;
        }

        private static bool MatchesName(Arguments args, MethodInfo x)
        {
            if (args.Keys.Contains(x.Name))
            {
                return true;
            }

            return args.Keys.Contains(x.DeclaringType.Name + "." + x.Name)
                   || args.Keys.Contains(x.DeclaringType.Namespace + "." + x.DeclaringType.Name + "." + x.Name);
        }
    }
}