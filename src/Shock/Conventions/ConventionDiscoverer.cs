using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            // Ok interesting, they've decided not to take a direct dependency, what magic can we do now...
            ModifyTaskDiscoveryConventions(conventions, allAvailableTypes);

            return conventions;
        }

        private static void ModifyTaskDiscoveryConventions(ActiveConventions conventions, List<Type> allAvailableTypes)
        {
            var taskDiscoverer = conventions.TaskDiscoverer as DefaultTaskDiscoverer;
            if (taskDiscoverer == null) return;

            var discoveryModifier =
                allAvailableTypes.Where(
                    x => x.GetMethods().Any(m => m.Name == "ConfigureTaskDiscovery"))
                    .ToList();

            foreach (var type in discoveryModifier)
            {
                var inst = Activator.CreateInstance(type);
                type.GetMethod("ConfigureTaskDiscovery")
                    .Invoke(inst, new object[] {taskDiscoverer.Matches, taskDiscoverer.ExcludeTypes});
            }
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
}