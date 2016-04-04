using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.ArgumentParsing;
using Shock.TaskDiscovery;

namespace Shock.AppDomainShims
{
    public static class DiscoverTasksAppDomainExtensions
    {
        public static List<MethodInfo> FindTasks(this IDiscoverTasks d, Arguments args)
        {
            return FindTasks(d, args, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IEnumerable<Type> AllTypes(this IEnumerable<Assembly> fromAssemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in fromAssemblies)
            {
                try
                {
                    var assemblyTypes = assembly.GetTypes();
                    types.AddRange(assemblyTypes);
                }
                catch{ /* ' V ' */}
            }
            return types;
        }

        public static List<MethodInfo> FindTasks(this IDiscoverTasks d, Arguments args, IEnumerable<Assembly> fromAssemblies)
        {
            return d.FindTasks(args, fromAssemblies.AllTypes().ToList());
        }
    }
}