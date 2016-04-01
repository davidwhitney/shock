using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.Execution;
using Shock.TaskDiscovery;

namespace Shock.Conventions
{
    public class ConventionDiscoverer : IConventionDiscoverer
    {
        public ActiveConventions AdjustConventions(ActiveConventions conventions, List<Type> allAvailableTypes)
        {
            if (ExecuteAnyConventionModifyingClasses(conventions, allAvailableTypes))
            {
                return conventions;
            }

            var taskDiscoverer = conventions.TaskDiscoverer as DefaultTaskDiscoverer;
            if (taskDiscoverer != null)
            {
                allAvailableTypes.ReflectAndExecute("ConfigureTaskDiscovery", taskDiscoverer.Matches, taskDiscoverer.ExcludeTypes);
            }

            var taskSelector = conventions.TaskSelector as SelectTasksToRun;
            if (taskSelector != null)
            {
                allAvailableTypes.ReflectAndExecute("ConfigureTaskSelection", taskSelector.Matches);
            }

            return conventions;
        }

        private static bool ExecuteAnyConventionModifyingClasses(ActiveConventions conventions, List<Type> allAvailableTypes)
        {
            var typesFound = new Dictionary<Type, ConstructorInfo>();
            foreach (var type in allAvailableTypes)
            {
                var conventionModifier = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                    .SingleOrDefault(c => c.GetParameters().Length == 1
                                          && c.GetParameters().First().ParameterType == typeof (ActiveConventions));

                if (conventionModifier != null)
                {
                    typesFound.Add(type, conventionModifier);
                }
            }

            if (!typesFound.Any()) return false;

            foreach (var conventionAdjuster in typesFound)
            {
                conventionAdjuster.Value.Invoke(new object[] {conventions});
            }

            return true;
        }
    }

    public static class ReflectAndExecuteExtensions
    {
        public static void ReflectAndExecute(this List<Type> types, string name, params object[] args)
        {
            var found = types.Where(x => x.GetMethods().Any(m => m.Name == name)).ToList();

            foreach (var type in found)
            {
                var inst = Activator.CreateInstance(type);
                type.GetMethod(name)
                    .Invoke(inst, args);
            }
        }
    }
}