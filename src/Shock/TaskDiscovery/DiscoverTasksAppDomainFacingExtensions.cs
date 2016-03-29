using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.TaskDiscovery
{
    public static class DiscoverTasksAppDomainFacingExtensions
    {
        public static List<MethodInfo> FindTasks(this IDiscoverTasks d, Arguments args)
        {
            return FindTasks(d, args, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static List<MethodInfo> FindTasks(this IDiscoverTasks d, Arguments args, IEnumerable<Assembly> fromAssemblies)
        {
            return d.FindTasks(args, fromAssemblies.SelectMany(a => a.GetTypes()).ToList());
        }
    }
}