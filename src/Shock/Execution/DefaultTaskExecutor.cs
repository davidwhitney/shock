using System.Reflection;
using Shock.ArgumentParsing;

namespace Shock.Execution
{
    public class DefaultTaskExecutor : IExecuteATask
    {
        public TaskStatus TryExecuteTask(MethodInfo method, Arguments args)
        {
            throw new System.NotImplementedException();
        }
    }
}