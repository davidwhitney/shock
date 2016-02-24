using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.TaskDiscovery
{
    public class DefaultTaskDiscoverer : IDiscoverTasks
    {
        public List<Func<MethodInfo, bool>> Matches { get; set; }
        public List<Func<Type, bool>> ExcludeTypes { get; set; }

        public DefaultTaskDiscoverer()
        {
            Matches = new List<Func<MethodInfo, bool>>
            {
                m => m.DeclaringType != null && m.DeclaringType.Name.Contains("Task"),
                m => m.DeclaringType != null && m.DeclaringType.Name.EndsWith("Tasks"),
                m => m.GetCustomAttributes().Any(a => a.GetType().Name == "TaskAttribute")
            };

            ExcludeTypes = new List<Func<Type, bool>>
            {
                t => t.Namespace.StartsWith("System."),
                t => t.FullName.StartsWith("System."),
                t => t.FullName.StartsWith("Accessibility."),
                t => t.Namespace.StartsWith("Microsoft."),
                t => t.Assembly.FullName.StartsWith("Shock, Version=")
            };
        } 

        public List<MethodInfo> FindTasks(Arguments args)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();
            FilterCandidateTypes(types);
            
            var methods = types.SelectMany(f => f.GetMethods(BindingFlags.Public | BindingFlags.Instance)).ToList();

            var tasks = new List<MethodInfo>();
            foreach (var method in methods)
            {
                if (Matches.Any(eval => eval(method)))
                {
                    tasks.Add(method);
                }
            }

            return tasks;
        }

        private void FilterCandidateTypes(List<Type> types)
        {
            var removals = new List<Type>();
            foreach (var type in types)
            {
                removals.AddRange(ExcludeTypes.Where(rule => rule(type)).Select(rule => type));
            }
            types.RemoveAll(t => removals.Contains(t));
        }
    }
}