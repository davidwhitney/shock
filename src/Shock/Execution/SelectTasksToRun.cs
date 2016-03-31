using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.ArgumentParsing;
using SelectionFunc = System.Func<System.Collections.Generic.List<System.Reflection.MethodInfo>, System.Collections.Generic.Dictionary<string, object>, System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>>;

namespace Shock.Execution
{
    public class SelectTasksToRun : ISelectTasksToRun
    {
        public List<SelectionFunc> Matches { get; }

        public SelectTasksToRun()
        {
            Matches = new List<SelectionFunc>
            {
                (candidates, args) => candidates.Where(x => x.DeclaringType != null && x.DeclaringType.Name == "DefaultTask" && x.Name == "Run"),
                (candidates, args) => candidates.Where(x => MatchesName(args, x))
            };
        }

        public List<MethodInfo> SelectTasksFrom(List<MethodInfo> tasksFromDomain, Arguments args)
        {
            var matchingTasks = new List<MethodInfo>();
            foreach (var selectionRule in Matches)
            {
                matchingTasks.AddRange(selectionRule(tasksFromDomain, args));
            }
            matchingTasks.RemoveAll(x => x == null);

            return matchingTasks;
        }

        private static bool MatchesName(Dictionary<string, object> args, MethodInfo x)
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