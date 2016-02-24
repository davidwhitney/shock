using System.Collections.Generic;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.TaskDiscovery
{
    public interface IDiscoverTasks
    {
        List<MethodInfo> FindTasks(Arguments args);
    }
}
