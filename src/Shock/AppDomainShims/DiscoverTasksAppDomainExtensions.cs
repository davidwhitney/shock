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

        public static List<Type> AllTypes(this IEnumerable<Assembly> fromAssemblies)
        {
            return fromAssemblies.SelectMany(a => a.GetTypes()).ToList();
        }

        public static List<MethodInfo> FindTasks(this IDiscoverTasks d, Arguments args, IEnumerable<Assembly> fromAssemblies)
        {
            return d.FindTasks(args, fromAssemblies.SelectMany(a => a.GetTypes()).ToList());
        }
    }
}