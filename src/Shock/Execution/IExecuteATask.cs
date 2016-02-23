using System.Collections.Generic;
using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Execution
{
    public interface IExecuteATask
    {
        TaskStatus TryExecuteTask(MethodInfo method, Arguments args);
    }
}