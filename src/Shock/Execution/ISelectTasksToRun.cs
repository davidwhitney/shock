using System.Collections.Generic;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Execution
{
    public interface ISelectTasksToRun
    {
        List<MethodInfo> SelectTasksFrom(List<MethodInfo> tasksFromDomain, Arguments args);
    }
}
